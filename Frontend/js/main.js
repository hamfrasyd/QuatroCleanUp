
const eventBaseUri = "http://quatro-api.mbuzinous.com/api/events"

//create en vueapp og tilføj en dataproporty
const app = Vue.createApp({
    data() {
        return {
            selectedEventId: null
        }
    },


    methods: {
        handleDelete(id){
            console.log("Skal slette event med ID:", id);
            this.selectedEventId = id;
            console.log("Efter sætning, selectedEventId er:", this.selectedEventId);
        },
        refreshList() {
            this.selectedEventId = null; // reset knappen
            this.$refs.eventListComponent.GetAllEvent(); // kald metoden igen
        }
    },


    computed: {
        myComputed() {
            return ''
        }
    }

})




