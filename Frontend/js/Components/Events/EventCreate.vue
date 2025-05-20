<script setup>
app.component('event-create', {
    template:
    /*html*/
    `
    <form class="card shadow p-4 needs-validation" novalidate @submit.prevent="createEventAsync"> 
        <h1>{{intro}}</h1>

        <div class="mb-3">
            <label for="titleForm" class="form-label">Title</label>
            <input v-model="title" type="text" class="form-control" id="titleForm" placeholder="e.g. Beach Clean-Up at Budva" required>
        <div class="invalid-feedback">This field is required.</div>
        </div>

        <div class="mb-3">
            <label for="descriptionForm" class="form-label">Description</label>
            <input v-model="description" type="text" class="form-control" id="descriptionForm" placeholder="e.g. Join us for a community beach clean-up at Budva to reduce plastic pollution and raise awareness about ocean waste. Gloves, bags, and refreshments will be provided." required>
            <div class="invalid-feedback">This field is required.</div>
        </div>

        <div class="mb-3">
            <label for="startTimeForm" class="form-label">Start Time</label>
            <input v-model="startTime" type="datetime-local" class="form-control" id="startTimeForm" placeholder="Start Time" required>
            <div class="invalid-feedback">This field is required.</div>
        </div>

        <div class="mb-3">
            <label for="endTimeForm" class="form-label">End Time</label>
            <input v-model="endTime" type="datetime-local" class="form-control" id="endTimeForm" placeholder="End Time" required>
            <div class="invalid-feedback">This field is required.</div>
        </div>
        
        <div class="form-check mb-3">
            <label class="form-check-label" for="familyFriendlyForm">Family Friendly Event</label>
            <input type="checkbox" value=0 v-model="familyFriendly" id="familyFriendlyForm" class="form-check-input" />
        </div>

        <div class="mb-3">
            <label for="participantsForm" class="form-label">Number of Participants</label>
            <input v-model="participants" type="text" class="form-control" id="participantsForm" placeholder="(Optional)">
        </div>

        <div class="mb-3">
            <label for="trashCollectedForm" class="form-label">Trash Collected</label>
            <input v-model="trashCollected" type="text" class="form-control" id="trashCollectedForm" placeholder="(Optional)">
        </div>
    
        <select v-model="statusId" class="form-select" aria-label="statusIdForm" required>
            <option disabled value="">-- Select status --</option>
            <option value="1">Upcoming</option>
            <option value="2">Ongoing</option>
            <option value="3">Completed</option>
            <option value="4">Cancelled</option>
        </select>

        <div class="mb-3">
            <label for="locationForm" class="form-label">Location Id</label>
            <input v-model="locationId" type="text" class="form-control" id="locationForm" placeholder="e.g. 1" required>
            <div class="invalid-feedback">This field is required.</div>
        </div>
     
        <div class="mb-3">
            <button type="submit" class="btn btn-success">Create Event</button>
        </div>
        
        <div v-if="statuscode" class="alert" :class="alertClass" role="alert">
            Status Code: {{ statuscode }}
            <br/>
            {{ statusMessage }}
        </div>
      </form>
     
      `,
      data() {
        return {
            intro: "Lav et nyt event",
            eventId: 0,
            title: "",
            description:"",
            startTime:"",
            endTime:"",
            familyFriendly: true,
            participants: "",
            trashCollected: "",
            statusId: "1",
            locationId: "",
            statuscode:"",
            statusMessage: ""
        }
    },
    methods: {
        async createEventAsync(domEvent){
              // 1. Access the form DOM element
            const form = domEvent.target.closest("form");

            // 2. Use browser's built-in validation
            if (!form.checkValidity()) {
                this.statuscode = "";
                this.statusMessage = "Please fill out all required fields.";
                    form.classList.add('was-validated'); // Optional: Bootstrap visual cue
                return;
            }
            //3. Send to api
            await axios.post(eventBaseUri, {
                eventId: this.eventId,
                title: this.title,
                description: this.description,
                startTime: this.startTime,
                endTime: this.endTime,
                familyFriendly: this.familyFriendly,
                participants: this.participants ? parseInt(this.participants) : 0,
                trashCollected: this.trashCollected ? parseInt(this.trashCollected) : 0,
                statusId: parseInt(this.statusId),
                locationId: parseInt(this.locationId)
            })
            .then(response => {
                this.statuscode = response.status;
                this.statusMessage = "Event created successfully!";
                this.eventId = response.eventId;
            })
            .catch(error => {
                this.statuscode = error.response?.status || "No status code.";
                this.statusMessage = "Error Message: " + error.message;
                console.log("Error in createEventAsync ", error);
            })
        }
    },
    computed: {
        // bruger denne til tjeke automatisk hvad status kode vi får tilbage fra api'en
        // og retunere css bootstrap class til Alert box, hvis alt går godt er den grøn, hvis skidt så rød osv..
        alertClass() {
            const code = parseInt(this.statuscode)
            if (code >= 200 && code < 300){
                return 'alert-success'
            } 
            if (code >= 300 && code < 400){
                return 'alert-primary'
            }
            if (code >= 400 && code < 500){
                return 'alert-danger'
                }
            if (code >= 500 && code < 600){
                return 'alert-warning'
            } 
            return 'alert-secondary'
        }
    }
})
</script>