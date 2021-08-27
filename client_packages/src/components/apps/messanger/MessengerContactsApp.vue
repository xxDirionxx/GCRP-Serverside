<template>
  <v-ons-page>
    <v-ons-toolbar>
      <div class="left">
        <v-ons-toolbar-button>
          <v-ons-back-button></v-ons-back-button>
        </v-ons-toolbar-button>
      </div>
      <div class="center">Kontakt senden</div>
    </v-ons-toolbar>
    <v-ons-list>
      <input style="width: 22.5vh; margin-left: 0.46875vh; height: 4.6875vh;" placeholder="Kontakt suchen..." v-model="filter" type="text" />
      <v-ons-list-item modifier="longdivider" tappable v-for="contact in search" :key="contact.number" v-on:click="pushSend(number, contact.name, contact.number)">
        <div class="center">
          <v-ons-icon v-if="contact.name.indexOf('000FAV') >= 0" style="margin-left: -1vh; color:#000000; font-size: 2vh; line-height: 0vh;" icon="ion-ios-star"></v-ons-icon>
          {{checkname(contact.name)}}
        </div>
        <div class="right">
          <v-ons-icon icon="md-mail-send" class="list-item__icon"></v-ons-icon>
        </div>
      </v-ons-list-item>
      <audio v-for="x in sendCount" v-bind:key="x" volume="0.1" visibility="visible" currentTime="0" autoplay controls type="audio/ogg" preload="auto" :src="playSound('sendSMS')"/>
    </v-ons-list>
  </v-ons-page>
</template>

<script>
  import Apps from "../../apps"
  import MessengerApp from "./MessengerApp"
  var filter;
  export default Apps.register({
    name: "ContactsApp",
    data() {
      return {
        contacts: [],
        filter: '',
        number: null,
        sendCount: 0
      }
    },
    computed: {
      search: function(){
        filter = this.filter.trim().toLowerCase();
        if (filter === '') return this.contacts;
        return this.contacts.filter(function(s) {
          return s.name.toLowerCase().indexOf(filter) >= 0;
        });
      }
    },
    methods: {
      pushSend(number, contactName, contactNumber) {
        if(contactName.indexOf('000FAV') >= 0){
            var contact = contactName.split('00FAV');
            contactName = contact[1];
        }
        this.sendCount = this.sendCount + 1
        this.triggerServer("sendPhoneMessage", number, "CONTACT$$" + contactName + "$$" + contactNumber)
        this.pageStack.pop()
        this.pageStack.pop()
        this.pageStack.pop()
        this.pageStack.push(MessengerApp)
      },
      playSound(data){
        if(data == 'sendSMS'){
            return Sounds.sendSMS()
        }
      },
      setContactListData(contacts) {
        this.contacts = JSON.parse(contacts)
      },
      checkname(name){
          if(name.indexOf('000FAV') >= 0){
              var contact = name.split('00FAV');
              return contact[1];
          }else{
              return name;
          }
      }
    },
    props: ['pageStack']
  })
</script>

<style lang="scss" scoped>
    audio{ 
        display:none;
    }
</style>