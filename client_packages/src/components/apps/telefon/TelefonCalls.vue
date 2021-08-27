<template>
  <v-ons-page>
    <v-ons-toolbar>
      <div class="left">
        <v-ons-toolbar-button>
          <v-ons-back-button></v-ons-back-button>
        </v-ons-toolbar-button>
      </div>
      <div class="center">Verlauf</div>
    </v-ons-toolbar>
    <v-ons-list id="calls">
      <v-ons-list-item modifier="longdivider" v-for="phone in phoneData" :key="phone.time">
        <div class="center" v-if="phone != null" v-on:click="call(phone.number)">
            <img v-if="phone.method != 'incoming' && phone.accepted == true"   style="width: 2.25vh;" :src="require('@/assets/smartphone/call/outgoing.png')">
            <img v-if="phone.method != 'incoming' && phone.accepted == false"  style="width: 2.25vh;" :src="require('@/assets/smartphone/call/outgoing_failed.png')">
            <img v-if="phone.method == 'incoming' && phone.accepted == true"    style="width: 2.25vh;" :src="require('@/assets/smartphone/call/incomming.png')">
            <img v-if="phone.method == 'incoming' && phone.accepted == false"   style="width: 2.25vh;" :src="require('@/assets/smartphone/call/incomming_failed.png')">
            <b style="margin-left: 4.5vh;position: absolute;font-size: 1.35vh">
              <span v-if="phone.contact != '' && phone.contact != null" style="font-size: 1.5vh;">{{checkname(phone.contact)}}</span><span v-else style="font-size: 1.5vh;">{{phone.number}}</span>
              <br>
              <span style="font-weight: normal;font-size: 1.05vh;">{{ phone.time }}</span>
            </b>
        </div>
      </v-ons-list-item>
      <v-ons-list-item  v-if="phoneData.length == 0">
        Es sind keine Anrufe vorhanden
      </v-ons-list-item >
    </v-ons-list>
  </v-ons-page>
</template>

<script>
    import Apps from "../../apps"

    export default Apps.register({
        name: "TelefonCalls",
        data() {
            return {
              phoneData: [],
              timeFormat: "",
            }
            //{contact: "Walter Hartz",time: "2019-12-01T19-00", method: 'incoming', accepted: true, number: 32834}
            //{contact: "Thomas Mayer",time: "2019-12-01T19-00", method: 'outgoing', accepted: false, number: 45004}
        },
        methods: {
            responsePhoneCalls(response){
              this.phoneData = JSON.parse(response)
              this.phoneData.reverse()
            },
            //showTime(time){
              //this.timeFormat = ""
              //this.timeFormat = time.split("T")[0].toString()
              //this.timeFormat = this.timeFormat.replace('-','.')
              //this.timeFormat = this.timeFormat.replace('-','.')
              //this.timeFormat += " "
              //this.timeFormat += time.split("T")[1].toString().replace('-',':')
              //return this.timeFormat
            //},
            call(number){
              if(number != null){
                this.triggerServer("callUserPhone", number)
              }
            },
            scrollToBottom() {
                setTimeout(() => {
                    const calls = document.getElementById("calls").childNodes
                    const length = this.phoneData.length - 1
                    calls[length].scrollIntoView(false)
                    console.log("scroll", length);
                }, 500)
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