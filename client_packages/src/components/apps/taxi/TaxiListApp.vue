<template>
  <v-ons-page>
    <v-ons-toolbar>
      <div class="left">
        <v-ons-toolbar-button>
          <v-ons-back-button></v-ons-back-button>
        </v-ons-toolbar-button>
      </div>
      <div class="center">Taxi</div>
    </v-ons-toolbar>

    <v-ons-list>
      <v-ons-list-header>Taxis im Dienst</v-ons-list-header>
      <v-ons-list-item v-if="taxis.length == 0">
        <div class="center">Keine Taxifahrer im Dienst</div>
      </v-ons-list-item>
      <v-ons-list-item v-else modifier="longdivider" tappable v-for="taxi in taxis" :key="taxi.name" v-on:click="push(taxi.name, taxi.number, taxi.price)">
        <div class="left">
          <img class="list-item__thumbnail" src="../../../assets/taxi.svg">
        </div>
        <div class="center">
          <span class="list-item__title">Preis: {{taxi.price}} $</span>
          <span class="list-item__subtitle">{{taxi.name}}</span>
        </div>
        <div class="right">
          <v-ons-icon icon="md-chevron-right" class="list-item__icon"></v-ons-icon>
        </div>
      </v-ons-list-item>
    </v-ons-list>
  </v-ons-page>
</template>

<script>
  import Apps from "../../apps"
  import TaxiContact from "./TaxiContact"

  export default Apps.register({
    name: "TaxiListApp",
    data() {
      return {
        taxis: [],
        driver: null
      }
    },
    methods: {
      push(contactName, contactNumber, contactPrice) {
        this.pageStack.push({ extends: TaxiContact, data(){ return { name: contactName, number: contactNumber, price: contactPrice }}})
      },
      responseTaxiList(taxiList) {
        this.taxis = JSON.parse(taxiList)
        console.log(JSON.parse(taxiList))
      }
    },
    mounted() {
      this.triggerServer("requestTaxiList")
    },
    props: ['pageStack']
  })
</script>