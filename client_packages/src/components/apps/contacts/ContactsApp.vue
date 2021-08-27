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
      <input style="width: 22.5vh; margin-left: 0.46875vh; height: 4.6875vh;" placeholder="Kontakt suchen..." v-model="filter" type="text" />
        <v-ons-list-item modifier="longdivider" tappable v-for="contact in search" :key="contact.number" v-on:click="pushEdit(contact.name, contact.number)">
          <div class="center">
            <v-ons-icon v-if="contact.name.indexOf('000FAV') >= 0" style="margin-left: -1vh; color:#000000; font-size: 2vh; line-height: 0vh;" icon="ion-ios-star"></v-ons-icon>
            {{checkname(contact.name)}}
          </div>
          <div class="right">
            <v-ons-icon icon="md-chevron-right" class="list-item__icon"></v-ons-icon>
          </div>
        </v-ons-list-item>
    </v-ons-list>

    <v-ons-fab modifier="mini" position='bottom right'>
      <v-ons-icon icon="md-plus" v-on:click="push"></v-ons-icon>
    </v-ons-fab>
  </v-ons-page>
</template>

<script>
  import Apps from "../../apps"
  import ContactsOverview from "./ContactsOverview"
  import ContactsAdd from "./ContactsAdd"
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
        search: function () {
            filter = this.filter.trim().toLowerCase();
            if (filter === '') return this.contacts;
            return this.contacts.filter(function (s) {
                return (
                    s.name.toLowerCase().includes(filter) ||
                    s.number.toString().toLowerCase().includes(filter)
                )
            });
        }
    },
    methods: {
      pushEdit(contactName, contactNumber) {
        this.pageStack.push({ extends: ContactsOverview, data(){ return { contactName: contactName, contactNumber: contactNumber }}})
      },
      push() {
        this.pageStack.push({ extends: ContactsAdd })
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