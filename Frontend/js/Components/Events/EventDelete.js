
app.component('event-delete', {
    props: ['eventId'],
    template:
    /*html*/
    `
    <div class="mb-3"> 
    <button type="button" class="btn btn-danger" v-if="eventId" @click="AgteDeleteEvent">Bekræft Slet Event {{ eventId }}</button>
    </div>
    `,
    mounted() {
        console.log("event-delete mounted med eventId:", this.eventId);
    },

    data() {
        return {
            statuskode:""
        }
    },
    methods: {
        AgteDeleteEvent(){
            console.log("Prøver at slette event med ID:", this.eventId);
            axios.delete(eventBaseUri + "/" + this.eventId)
            .then(response => {
                console.log("Event slettet", response.data);
                this.$emit('deleted');
            })
            .catch(error => {
                this.statuskode = error.response?.status || error.message;
                console.log("Fejl i deleteEvent", error);
            });
        }
    },
    computed: {
        myComputed() {
            return ''
        }
    }
})