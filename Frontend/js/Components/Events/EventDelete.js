
    app.component('event-delete', {
        props: ['eventId'],
        template:
        /*html*/
    `    <button v-if="eventId" @click="AgteDeleteEvent">Bekræft Slet Event {{ eventId }}</button>
    `
        ,
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