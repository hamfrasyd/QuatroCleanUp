
app.component('event-getall', {
    emits: ['emit-delete-event'],
    template:
    /*html*/
    `<button class="btn btn-primary" v-on:click="GetAllEvent">Get All</button>
    <ol v-if="eventList.length > 0">
    <li v-for="event in eventList" v-bind:key="event.eventId">
        Title {{event.title}}: description {{event.description}} : startTime {{event.startTime}} : endTime {{event.endTime}} : 
        familyFriendly {{event.familyFriendly}} : participants {{event.participants}} : trashCollected {{event.trashCollected}} : 
        statusId {{event.statusId}} : locationId {{event.locationId}} : statuskode {{event.statuskode}},  
        
         <button @click="emitDeleteEvent(event.eventId)">Delete</button>
    </li>
</ol>       
<div v-else style="font-style: italic;">No events to show</div>`
    ,
    

    data() {
        return {
            eventList: [],
            statuskode:""
        }
    },


    methods: {
        GetAllEvent(){
           axios.get(eventBaseUri)
        .then(response => {
            this.statuskode = response.status,
            this.eventList = response.data

        })
        .catch(error => {
            this.statuskode = error.response?.status || "Fejl"
            console.log("Fejl i createEvent", error)
        })
           
        },

        emitDeleteEvent(id) {
            console.log("Udsender delete-event med ID:", id);
            this.$emit('emit-delete-event', id);
          }
    },


    computed: {
        myComputed() {
            return ''
        }
    }

})