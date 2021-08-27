<template>
    <v-ons-page>
        <v-ons-toolbar>
            <div class="left">
                <v-ons-toolbar-button>
                    <v-ons-back-button></v-ons-back-button>
                </v-ons-toolbar-button>
            </div>
            <div class="center">Banking</div>
        </v-ons-toolbar>
        <v-ons-list>
            <v-ons-list-header>Guthaben: <span style="font-size:1.4vh;color:black;font-weight:bold">${{convert(balance)}}</span></v-ons-list-header>
        </v-ons-list>
        <v-ons-list>
            <v-ons-list-header>Kontobewegungen</v-ons-list-header>
            <v-ons-list-item modifier="longdivider" v-for="(overview,index) in bankOverview" :key="index">
                <div class="left" style="font-size:1.2vh !important">
                    {{overview.name}}
                </div>
                <div class="center">      
                </div>
                <div class="right" style="font-size:1.2vh !important">
                    <span v-if="overview.value == 0">{{convert(overview.value)}}</span>
                    <span v-if="overview.value >= 1 && overview.name == 'Geldautomat - Auszahlung'" style="color:red">{{convert(overview.value)}}</span>
                    <span v-else-if="overview.value >= 1" style="color:green">{{convert(overview.value)}}</span>
                    <span v-if="overview.value <= -1" style="color:red">{{convert(overview.value)}}</span>
                 </div>
            </v-ons-list-item>
        </v-ons-list>
    </v-ons-page>
</template>

<script>
  import Apps from "../../apps"

  export default Apps.register({
    name: "BankAppOverview",
    data() {
        return {
            balance: 0,
            bankOverview: []
      }
    },
      methods: {
          convert(money) {
              return money.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ".");
          },
          responseBankAppOverview(balance, history) {
              this.balance = balance;
              this.bankOverview = JSON.parse(history);
          }
          
    },
      mounted() {
          this.triggerServer("requestBankAppOverview")
    },
    props: ['pageStack']
  })
</script>