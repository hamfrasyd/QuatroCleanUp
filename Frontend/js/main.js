
const baseUri = "https://localhost:7124/api/Event"

//create en vueapp og tilføj en dataproporty
const app = Vue.createApp({
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
            statuskode:"",
        }
    },
    methods: {
        createEvent(){
            axios.post(baseUri, {
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
                this.statuskode = response.status
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




