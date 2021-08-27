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

    <section style="margin: 20px">
      <p style="text-align: center; font-size: 20px; margin-top: 40px; color: #000;">{{name}}</p>
      <p style="text-align: center; font-size: 15px; margin-top: 5px; color: #000;">{{price}} $</p>
      <br />
      <input style="width:100%; margin-right: 10px;" placeholder="Dein Standort / Telefonnummer" v-model="message" type="text" />
      <br />
      <v-ons-button modifier="large" style="margin: 6px 0" v-on:click="requestTaxi">Taxi anfordern</v-ons-button>
    </section>
  </v-ons-page>
</template>

<script>
import Apps from "../../apps"

export default Apps.register({
    name: "TaxiContact",
    data() {
        return {
            name: "",
            message: "",
            number: null,
            price: null
        }
    },
    methods: {
        requestTaxi() {
          if(this.name == "" || this.message == "" || this.price == null) return
          this.triggerServer("requestTaxiDriver", this.name, this.message, this.price)
        }
    },
    props: ['pageStack']
})
</script>