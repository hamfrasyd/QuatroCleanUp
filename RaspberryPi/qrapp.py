from flask import Flask, render_template, request, redirect, url_for
from picamera2 import Picamera2
from PIL import Image
from pyzbar.pyzbar import decode
import requests
import time

app = Flask(__name__)

@app.route('/')
def index():
    return render_template('index.html')

@app.route('/take_picture', methods=['POST'])
def take_picture():
    picam2 = Picamera2()
    picam2.start()
    time.sleep(2)  # Allow camera to initialize
    image_path = "image.jpg"
    picam2.capture_file(image_path)
    picam2.stop()

    img = Image.open(image_path)
    decoded_objects = decode(img)

    if decoded_objects:
        qr_data = decoded_objects[0].data.decode('utf-8')
        api_url = "http://your-api-url/api/attendance"  # Replace with your API URL
        response = requests.post(api_url, json={"eventId": qr_data})
        if response.status_code == 200:
            return f"Success: Event ID {qr_data} sent to API"
        else:
            return f"Failed to send to API: {response.text}"
    else:
        return "No QR code found"
if __name__ == '__main__':
    app.run(host='0.0.0.0', port=5000)
