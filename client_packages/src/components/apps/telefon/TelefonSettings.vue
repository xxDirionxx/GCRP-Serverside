<template>
  <v-ons-page>
    <v-ons-toolbar>
      <div class="left">
        <v-ons-toolbar-button>
          <v-ons-back-button></v-ons-back-button>
        </v-ons-toolbar-button>
      </div>
      <div class="center">Telefon</div>
    </v-ons-toolbar>
    <v-ons-list>
      <v-ons-list-header>Settings</v-ons-list-header>
      <v-ons-list-item modifier="longdivider">
        <div class="center"><b>Guthaben:</b> {{ phoneData.guthaben }}</div>
      </v-ons-list-item>
      <v-ons-list-item modifier="longdivider">
        <div class="center"><b>Nummer:</b> {{ phoneData.number }}</div>
      </v-ons-list-item>
    </v-ons-list>
  </v-ons-page>
</template>

<script>
    import Apps from "../../apps"

    export default Apps.register({
        name: "TelefonSettings",
        data() {
            return {
              phoneData: []
            }
        },
        methods: {
            responsePhoneData(response){
              this.phoneData = JSON.parse(response)
            }
        },
        mounted(){
          this.triggerServer("requestPhoneData")
        },
        props: ['pageStack']
    })
</script>