const pictureApiUrl = 'http://quatro-api.mbuzinous.com/api'

app.component('picture-upload', {
  template: `
    <div class="picture-upload"> 
      <h2>Upload Picture</h2>
      <input type="file" @change="handleFileSelection" accept="image/*" />
      <br/>
      <input type="text" v-model="description" placeholder="Description (Not required)" />
      <br/>
      <button @click="uploadPicture">Upload</button>
      <br/>
      <p v-if="selectedImageFile">Selected file: {{ selectedImageFile.name }}</p>
      <img v-if="previewImageUrl" :src="previewImageUrl" style="width: 20%; height: 20%;" />
      <p v-if="statusMessage">{{ statusMessage }}</p>
    </div>
  `,
  data() {
    return {
      eventId: 5,               // eller bind via prop
      description: '',   
      pictureDataBase64String: null,        // base64-streng uden prefix
      selectedImageFile: null,
      previewImageUrl: null,
      statusMessage: ''
    };
  },
  methods: {
    handleFileSelection(imageInputEvent) {
      const imageFile  = imageInputEvent.target.files[0];
      this.statusMessage = null;

      if (!imageFile) {
        // No file chosen → clear everything
        this.selectedImageFile = null;
        this.previewImageUrl = null;
        this.imageBase64Data = null;
        return;
      }

      this.selectedImageFile = imageFile ;
      // Preview
      this.previewImageUrl = URL.createObjectURL(imageFile );

      // Read som DataURL og strip prefix
      const reader = new FileReader();
      reader.onload = () => {
        // reader.result er f.eks. "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAA..."
        const dataUrl = reader.result;
        // DataURL format is "data:image/png;base64,iVBORw..."
        const base64 = dataUrl.split(',')[1];
        this.pictureDataBase64String = base64;
      };
      reader.readAsDataURL(imageFile );
    },
    uploadPicture() {
      if (!this.pictureDataBase64String) {
        this.statusMessage = 'Please select and load a file first.';
        return;
      }
      // Byg det JSON-objekt, som Picture-klassen forventer
      const pictureDTO = {
        eventId: this.eventId,
        pictureData: this.pictureDataBase64String,
        description: this.description.trim() || 'No description'
      };
        axios.post(pictureApiUrl+'/pictures', pictureDTO)
        .then(response => 
        {
             console.log("File uploaded successfully! Status code: " + response.status + " Response Data: " + response.data);
             this.statusMessage = "File uploaded successfully!"
        })
        .catch(error => {
             this.statusMessage = "Status code: " + response.status + " Error Message: " + error.message
             console.log("Status code: " + response.status + " Error Message: " + error.message);   
        })
        // Reset efter upload
        this.selectedImageFile = null;
        this.previewImageUrl = null;
        this.pictureDataBase64String = null;
        this.description = '';
    }
  },
  /*
    In case parent unmounts us while a preview is active:
    clean up the object URL to avoid memory leaks.
  */
  beforeUnmount() {
    if (this.previewImageUrl) {
      URL.revokeObjectURL(this.previewImageUrl);
    }
  }
});
