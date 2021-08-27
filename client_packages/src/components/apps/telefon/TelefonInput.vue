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
      <v-ons-list-header>Anrufen</v-ons-list-header>
      <v-ons-list-item modifier="longdivider">
        <div class="center">
          <input type="number" style="width:100%; margin-right: 10px;" placeholder="Nummer eingeben" v-model="number" />
        </div>
      </v-ons-list-item>
      <v-ons-list-item modifier="longdivider">
        <v-ons-button style="width:100%; margin-right: 10px; margin-top: -1px;" class="color-background blue" modifier="large" v-on:click="CallUser">Anrufen</v-ons-button>
      </v-ons-list-item>
    </v-ons-list>
  </v-ons-page>
</template>

<script>
    import Apps from "../../apps"

    export default Apps.register({
        name: "TelefonInput",
        data() {
            return {
              number: null
            }
        },
        methods: {
            CallUser(){
              if(this.number == null) return
              if(this.number <= 0) return
              this.triggerServer("callUserPhone", this.number)
            }
        },
        props: ['pageStack']
    })
</script>