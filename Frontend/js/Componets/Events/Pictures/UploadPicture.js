app.component('picture-upload', {
  template: `
    <div>
      <h2>Upload Picture</h2>
      <input type="file" @change="onFileChange" accept="image/*" />
      <input type="text" v-model="description" placeholder="Description" />

      <button @click="uploadPicture">Upload</button>

      <p v-if="selectedFile">Selected file: {{ selectedFile.name }}</p>
      <img v-if="previewUrl" :src="previewUrl" class="w-32 h-auto my-2" />
      <p v-if="statusMessage">{{ statusMessage }}</p>
    </div>
  `,
  data() {
    return {
      eventId: 5,               // eller bind via prop
      description: '',          
      selectedFile: null,
      previewUrl: null,
      pictureData: null,        // base64-streng uden prefix
      statusMessage: ''
    };
  },
  methods: {
    onFileChange(e) {
      const file = e.target.files[0];
      if (!file) {
        this.selectedFile = null;
        this.previewUrl = null;
        this.pictureData = null;
        return;
      }
      this.selectedFile = file;
      // Preview
      this.previewUrl = URL.createObjectURL(file);

      // Read som DataURL og strip prefix
      const reader = new FileReader();
      reader.onload = () => {
        // reader.result er f.eks. "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAA..."
        const dataUrl = reader.result;
        const base64 = dataUrl.split(',')[1];
        this.pictureData = base64;
      };
      reader.readAsDataURL(file);
    },

    async uploadPicture() {
      if (!this.pictureData) {
        this.statusMessage = 'Please select and load a file first.';
        return;
      }
      // Byg det JSON-objekt, som Picture-klassen forventer
      const payload = {
        eventId: this.eventId,
        pictureData: this.pictureData,
        description: this.description.trim() || 'No description'
      };

      try {
        const response = await axios.post(
          'http://quatro-api.mbuzinous.com/api/pictures',
          payload,
          { headers: { 'Content-Type': 'application/json' } }
        );
        this.statusMessage = 'File uploaded successfully!';
        console.log('Response:', response.data);
        // Reset efter upload
        this.selectedFile = null;
        this.previewUrl = null;
        this.pictureData = null;
        this.description = '';
      } catch (err) {
        this.statusMessage = 'Error uploading file: ' + (err.response?.data || err.message);
        console.error(err);
      }
    }
  },
  beforeUnmount() {
    if (this.previewUrl) {
      URL.revokeObjectURL(this.previewUrl);
    }
  }
});
