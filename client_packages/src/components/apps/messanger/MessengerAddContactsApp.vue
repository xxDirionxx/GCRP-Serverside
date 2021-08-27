<template>
  <v-ons-page>
    <v-ons-toolbar>
      <div class="left">
        <v-ons-toolbar-button>
          <v-ons-back-button></v-ons-back-button>
        </v-ons-toolbar-button>
      </div>
      <div class="center">Kontakte</div>
    </v-ons-toolbar>
    <v-ons-list>
      <input style="width: 22.5vh; margin-left: 0.46875vh; height: 4.6875vh;" placeholder="Kontakt oder Nummer eingeben" v-model="filter" type="text" />
      <v-ons-list-item modifier="longdivider" tappable v-if='filter != "" && onlynumber(filter) == true' v-on:click="pushSend(filter,filter)">
        <div class="center">
          {{filter}}
        </div>
        <div class="right">
          <v-ons-icon icon="md-mail-send" class="list-item__icon" style="color: rgb(0, 118, 255)"></v-ons-icon>
        </div>
      </v-ons-list-item>
      <v-ons-list-item modifier="longdivider" tappable v-for="contact in search" :key="contact.number" v-on:click="pushSend(contact.number,checkname(contact.name))">
        <div class="center">
          <v-ons-icon v-if="contact.name.indexOf('000FAV') >= 0" style="margin-left: -1vh; color:#000000; font-size: 2vh; line-height: 0vh;" icon="ion-ios-star"></v-ons-icon>
          {{checkname(contact.name)}}
        </div>
        <div class="right">
          <v-ons-icon icon="md-mail-send" class="list-item__icon" style="color: rgb(0, 118, 255)"></v-ons-icon>
        </div>
      </v-ons-list-item>
    </v-ons-list>
  </v-ons-page>
</template>

<script>
  import Apps from "../../apps"
  import MessageOverviewApp from "./MessageOverviewApp"
  var filter;
  export default Apps.register({
    name: "ContactsApp",
    data() {
      return {
        contacts: [],
        filter: ''
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
      pushSend(contactNumber, contactName) {
        this.pageStack.push({ extends: MessageOverviewApp, data() { return { msgPartner: contactName, msgPartnerNumber: contactNumber } } })
      },
      onlynumber(check){
        const digits_only = string => [...string].every(c => '0123456789'.includes(c));
        return digits_only(check)
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