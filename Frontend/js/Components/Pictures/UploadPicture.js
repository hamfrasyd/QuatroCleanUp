const pictureApiUrl = 'http://quatro-api.mbuzinous.com/api'

app.component('picture-upload', {
  /*props: ['eventId'],*/
  template: 
  /*html*/
  `
    <form class="card shadow p-4 needs-validation" novalidate @submit.prevent="uploadPictureAsync">
      <h2>Upload Picture For <strong>Event-Id: {{eventId}}</strong></h2>
      <div class="mb-3">
        <input class="form-control" type="file" id="pictureUploadForm" @change="handleFileSelection" accept="image/*" required>
        <div class="invalid-feedback">Please select an image.</div>
      </div>
        
      <div class="mb-3">
        <label for="pictureDescription" class="form-label">Description (Optional)</label>
        <textarea v-model="description" class="form-control" id="pictureDescription" rows="3"  placeholder="Group photo of our beach cleanup crew holding up bags of collected plastic waste."></textarea>
      </div>

      <div class="mb-3">
        <button type="submit" class="btn btn-success" @click="uploadPictureAsync">Upload</button>
      </div>

      <div v-if="statusMessage && statusMessage.includes('File uploaded successfully!')" class="alert alert-success mb-3" role="alert">
        <p>{{ statusMessage }}</p>
      </div>
      <div v-else-if="statusMessage" class="alert alert-danger mb-3" role="alert">
        <p>{{ statusMessage }}</p>
      </div>

      <div class="mb-3">
        <img v-if="previewImageUrl" :src="previewImageUrl" class="img-fluid img-thumbnail" style="max-width: 30rem;" alt="Image preview" />
      </div>
    </div>
  `,
  data() {
    return {
      eventId: 5,// Skift over til at binde via propm år vi har Event Details kørende
      description: '',   
      pictureDataBase64String: null,// base64-streng uden prefix
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
    async uploadPictureAsync(domEvent) {
      // 1. Access "form" DOM element
      const form = domEvent.target.closest("form");

      // 2. Validere form input
      if (!form.checkValidity() || !this.pictureDataBase64String) {
        form.classList.add("was-validated");
        return;
      }

      // 3. Byg det JSON-objekt, som Picture-klassen forventer
      const pictureDTO = {
        eventId: this.eventId,
        pictureData: this.pictureDataBase64String,
        description: this.description.trim() || 'No description'
      };

      //4. Kald api'en
      await axios.post(pictureApiUrl+'/pictures', pictureDTO)
      .then(response => 
      {
            console.log("File uploaded successfully! Status code: " + response.status + " Response Data: " + response.data);
            this.statusMessage = "File uploaded successfully!"
      })
      .catch(error => {
            this.statusMessage = "Status code: " + response.status + " Error Message: " + error.message
            console.log("Status code: " + response.status + " Error Message: " + error.message);   
      })
      
      // 5. Reset efter upload
      this.selectedImageFile = null;
      this.previewImageUrl = null;
      this.pictureDataBase64String = null;
      this.description = '';
      form.classList.remove("was-validated");
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
