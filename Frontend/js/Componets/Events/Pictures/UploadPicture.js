app.component('picture-upload', {
    template: 
    `
        <div>
            <h2>Upload Picture</h2>
            <input type="file" @change="onFileChange" />
            <input type="text" v-model="description" placeholder="Description" />

            <button @click="uploadPicture">Upload</button>
            <p v-if="selectedFile">Selected file: {{ selectedFile.name }}</p>
            <img v-if="selectedFile" :src="previewUrl" class="w-32 h-auto my-2" />
            <p v-if="statusMessage">{{ statusMessage }}</p>
        </div>
    `,
    data() {
        return {
            eventId: 5,
            pictureData: null,
            description: 'No description',
            selectedFile: null,
            statusMessage: '',
            previewUrl: null
        }
    },
    methods: {
        uploadPicture() {
            if (!this.selectedFile) {
                this.statusMessage = 'Please select a file to upload.';
                return;
            }
            const formData = new FormData();
            formData.append('eventId', this.eventId);
            formData.append('pictureData', this.selectedFile);
            formData.append('description', this.description);

            axios.post(`http://quatro-api.mbuzinous.com/api/pictures`, formData, {
                headers: {
                    'Content-Type': 'multipart/form-data'
                }
            })
            .then(response => {
                this.statusMessage = 'File uploaded successfully!';
                console.log(response.data);
            })
            .catch(error => {
                this.statusMessage = 'Error uploading file.';
                console.error(error);
            });
        },
        onFileChange (e) {
        this.selectedFile = e.target.files[0] || null
        this.previewUrl = this.selectedFile ? URL.createObjectURL(this.selectedFile) : null
        },
        beforeUnmount () {
        // avoid memory leaks
        if (this.previewUrl) URL.revokeObjectURL(this.previewUrl)
        }
    }
}
)