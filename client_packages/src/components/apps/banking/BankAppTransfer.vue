<template>
    <v-ons-page>
        <v-ons-toolbar>
            <div class="left">
                <v-ons-toolbar-button>
                    <v-ons-back-button></v-ons-back-button>
                </v-ons-toolbar-button>
            </div>
            <div class="center">Überweisung</div>
        </v-ons-toolbar>
         <v-ons-list>
             <v-ons-list-header>Überweisung an</v-ons-list-header>
             <v-ons-list-item modifier="longdivider">
             <div class="center">
                <v-ons-input placeholder="Kontoinhaber" float v-model="playerbank"></v-ons-input>
             </div>
             </v-ons-list-item>
             <v-ons-list-item modifier="longdivider">
             <div class="center">
                <v-ons-input placeholder="Betrag" float v-model="amount" type="number"></v-ons-input>
             </div>
             </v-ons-list-item>
              <v-ons-list-item modifier="longdivider">
             <div class="center">
                <v-ons-button modifier="large" v-on:click="transfer(playerbank,amount)">Überweisen</v-ons-button>
             </div>
             </v-ons-list-item>
             <v-ons-list-item modifier="longdivider">
             <div class="center">
                <span style="color:red">{{error}}</span>
             </div>
             </v-ons-list-item>
        </v-ons-list>

    </v-ons-page>
</template>

<script>
  import Apps from "../../apps"

  export default Apps.register({
    name: "BankAppTransfer",
    data() {
        return {
          playerbank:"",
          amount:"",
          error:"",
          bankingmaxcap: 0,
          bankingmincap: 0,
          tax:0
      }
    },
      methods: {
          convert(money) {
              return money.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ".");
          },
          responseBankingCap(maxcap,mincap){
              this.bankingmaxcap = maxcap;
              this.bankingmincap = mincap;
          },
          transfer(player,amount)
          {
                this.error = "";
                if(player==""||amount==""||amount==0){
                this.error="Bitte die Felder ausfüllen";
                                return;
                }
               if(/\d/.test(player)){
                this.error="Format des Kontoinhabers falsch";
                    return;
                }

              if (!this.isInt(amount)){
                this.error="Betrag ist keine ganze Zahl";
                    return;
              }

              amount = parseInt(amount)
              if (amount < this.bankingmincap) {
                   this.error = "Überweisungen erst ab $" + this.convert(this.bankingmincap) + " möglich";
                      return;
               }

              if (amount > this.bankingmaxcap){
                  this.error = "Zu Ihrer Sicherheit können nur Beträge bis $" + this.convert(this.bankingmaxcap)+" überwiesen werden";
                    return;
                }

                this.triggerServer("bankingAppTransfer",player,amount);
                this.pageStack.pop();
                
          },
          isInt(value) {
            return !isNaN(value) && (function(x) { return (x | 0) === x; })(parseFloat(value))
          }
    },
    mounted() {
        this.triggerServer("requestBankingCap")
    },
    props: ['pageStack']
  })
</script>