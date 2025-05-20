from flask import Flask, render_template, Response, request
from picamera2 import Picamera2
from picamera2.errors import CameraUnavailable      # ← ændret linje
from PIL import Image
from pyzbar.pyzbar import decode
import requests, cv2

try:
    picam2 = Picamera2()
except CameraUnavailable as e:
    raise SystemExit(f"No camera detected → {e}")

if not Picamera2.global_camera_info():
    raise SystemExit("Picamera2 found 0 cameras. Check ribbon cable & enable camera.")

app = Flask(__name__)

config = picam2.create_preview_configuration(main={"format": "BGR888"})
picam2.configure(config)
picam2.start()

# ───────────────────────────────────────── Flask app ──
app = Flask(__name__)

def generate_frames():
    """Yield MJPEG frames from the running Picamera2."""
    while True:
        frame = picam2.capture_array()                 # ndarray (BGR)
        ok, jpg = cv2.imencode(".jpg", frame)          # → bytes
        if not ok:
            continue
        yield (b"--frame\r\n"
               b"Content-Type: image/jpeg\r\n\r\n" +
               jpg.tobytes() + b"\r\n")

@app.route("/")
def index():
    return render_template("index.html")               # must contain <img src="/video_feed">

@app.route("/video_feed")
def video_feed():
    return Response(generate_frames(),
                    mimetype="multipart/x-mixed-replace; boundary=frame")

@app.route("/take_picture", methods=["POST"])
def take_picture():
    """Capture one frame, decode first QR code, forward to API."""
    frame = picam2.capture_array()
    rgb   = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
    img   = Image.fromarray(rgb)

    codes = decode(img)
    if not codes:
        return "No QR code found", 400

    event_id = codes[0].data.decode("utf-8")
    API_URL  = "http://YOUR_BACKEND_HOST/api/attendance"  # <-- edit me

    try:
        resp = requests.post(API_URL, json={"eventId": event_id}, timeout=3)
        resp.raise_for_status()
    except requests.RequestException as exc:
        return f"API error: {exc}", 502

    return f"Success: {event_id}", 200

# ──────────────────────────────────────────────────────
if __name__ == "__main__":
    # Expose on all interfaces; change port if you already use :5000
    app.run(host="0.0.0.0", port=5000, debug=False)
