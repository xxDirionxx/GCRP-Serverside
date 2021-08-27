<template>
  <v-ons-page>
    <v-ons-toolbar>
      <div class="left">
        <v-ons-toolbar-button>
          <v-ons-back-button></v-ons-back-button>
        </v-ons-toolbar-button>
      </div>
      <div class="center">Life Invader</div>
    </v-ons-toolbar>
    <div>

    <v-ons-list v-if="AdvertisingData.length == 0">
      <v-ons-list-item>
        <div class="center">Keine Werbung vorhanden</div>
      </v-ons-list-item>
    </v-ons-list>

      <v-ons-card v-else v-for="ads in AdvertisingData" :key="ads.id">
        <div class="content">
          <v-ons-list>
            <v-ons-list-header>{{ads.title}}</v-ons-list-header>
            <v-ons-list-item v-if="ads.content.indexOf('$$$$') >= 0">
              <p style="font-size: 1.3vh;">{{advert("MSG",ads.content)}}</p>
              <p style="display: block; margin-top: 0.9375vh; font-size: 1.5vh; margin-top: -0.5svh;">Telefon: {{advert("NUMBER",ads.content)}}
                  <button style="vertical-align: middle; color: #696969; margin-left: 1.5vh; margin-top: 1.1vh;" class="fas fa-phone fa-1x" v-on:click="call(ads.content)"><v-ons-icon icon="md-address-book" class="list-item__icon"></v-ons-icon></button>
                  <button style="vertical-align: middle; color: #696969; margin-top: 1.1vh;" class="far fa-comments fa-1x" v-on:click="sendsms(ads.content)"><v-ons-icon icon="md-address-book" class="list-item__icon"></v-ons-icon></button>
              </p>
            </v-ons-list-item>
            <v-ons-list-item v-else>
              <p>{{ads.content}}</p>
            </v-ons-list-item>
          </v-ons-list>
        </div>
      </v-ons-card>
    </div>
  </v-ons-page>
</template>

<script>
    import Apps from "../apps"
    import MessengerMessageApp from "./messenger/MessengerMessageApp"
    import MessageOverviewApp from "./messenger/MessageOverviewApp"

    export default Apps.register({
        name: "LifeInvaderApp",
        data() {
            return {
              AdvertisingData: []
            }
        },
        methods: {
            updateLifeInvaderAds(ads) {
                this.AdvertisingData = JSON.parse(ads)
            },
            advert(modus, advertdata){
                var advert = advertdata.split('$$$$');
                if(modus == "MSG"){
                    return advert[0];
                }else if(modus == "NUMBER"){
                    return advert[1];
                }
            },
            call(advertdata){
                var advert = advertdata.split('$$$$');
                this.triggerServer("callUserPhone", advert[1])
            },
            sendsms(advertdata){
                var advert = advertdata.split('$$$$');
                this.pageStack.push({ extends: MessageOverviewApp, data() { return { msgPartner: advert[1], msgPartnerNumber: advert[1] } } })
            }
        },
        mounted() {
            this.triggerServer("requestAd")
        },
        props: ['pageStack']
    })
</script>

<style>
  section {
      margin: 1.875vh;
  }
</style>
