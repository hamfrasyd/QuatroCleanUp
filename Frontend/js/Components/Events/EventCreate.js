
app.component('event-create', {
    template:
    /*html*/
    `<div class="event-create">
    
    <h1>{{intro}}</h1>
    <input v-model="title" placeholder="title">
    <br>
    <input v-model="description" placeholder="description">
    <br>
    <input v-model="startTime" type="datetime-local" id="startTime" name="startTime">
    <br>
    <input v-model="endTime" type="datetime-local" id="endTime" name="endTime">
    <br>
    <label for="familyFriendly">Family Friendly:</label>
    <input type="checkbox" v-model="familyFriendly" name="familyFriendly">
    <br>
    <input v-model="participants" placeholder="participants">
    <br>
    <input v-model="trashCollected" placeholder="trashCollected">
    <br>
    <input v-model="statusId" placeholder="statusId">
    <br>
    <input v-model="locationId" placeholder="locationId">
    <br>

    <button v-on:click="createEvent" >Create Event</button>

    <div>
        {{title}}
        {{description}}
        {{startTime}}
        {{endTime}}
        {{familyFriendly}}
        {{participants}}
        {{trashCollected}}
        {{statusId}}
        {{locationId}}
      </div>

      <h1>status : {{statuskode}}</h1>
      </div>`,
      data() {
        return {
            intro: "Lav et nyt event",
            eventId: 0,
            title: "",
            description:"",
            startTime:"",
            endTime:"",
            familyFriendly: false,
            participants: "",
            trashCollected: "",
            statusId: "",
            locationId: "",
            statuskode:""
        }
    },
    methods: {
        createEvent(){
            axios.post(eventBaseUri, {
                eventId: this.eventId,
                title: this.title,
                description: this.description,
                startTime: this.startTime,
                endTime: this.endTime,
                familyFriendly: this.familyFriendly,
                participants: parseInt(this.participants),  
                trashCollected: parseInt(this.trashCollected),
                statusId: parseInt(this.statusId),
                locationId: parseInt(this.locationId),
            })
            .then(response => {
                this.statuskode = response.status,
                this.eventId = response.eventId
            })
            .catch(error => {
                this.statuskode = error.response?.status || "Fejl"
                console.log("Fejl i createEvent", error)
            })
        }

        
    },
    computed: {
        myComputed() {
            return ''
        }
    }

})