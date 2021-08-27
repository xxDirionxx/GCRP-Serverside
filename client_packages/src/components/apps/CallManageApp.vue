<template>
  <v-ons-page>
    <div v-if="accepted == false">
      <div style="text-align: center; font-size: 1.875vh; margin-top: 3.75vh; color: #000;">
        <p v-if="callData.method == 'incoming'">Eingehender Anruf</p>
        <p v-else>Ausgehender Anruf</p>
      </div>
      <div style="text-align: center; font-size: 1.40625vh; margin-top: 0.46875vh; color: #000;">
          <span v-if="callData.name !== null">{{checkname(callData.name)}}</span>
          <span v-else-if="callData.name === null">{{callData.caller}}</span>
      </div>
 
      <ons-fab v-if="callData.method == 'incoming'" v-on:click="declineCall" position="bottom left" style="background-color: red; color: white; width: 3.75vh; height: 3.75vh; line-height: 3.75vh; font-size: 2.5vh; margin: 0; padding: 0;">
        <ons-icon icon="md-phone-end" style="font-size: 2.5vh; margin: 0; padding: 0; line-height: 0vh;"></ons-icon>
      </ons-fab>
      <ons-fab v-else v-on:click="declineCall" position="bottom center" style="background-color: red; color: white; width: 3.75vh; height: 3.75vh; line-height: 3.75vh; font-size: 2.5vh; margin: 0; padding: 0;">
        <ons-icon icon="md-phone-end" style="font-size: 2.5vh; margin: 0; padding: 0; line-height: 0vh;"></ons-icon>
      </ons-fab>
 
      <ons-fab v-if="callData.method == 'incoming'" v-on:click="acceptCall" position="bottom right" style="background-color: green; color: white; width: 3.75vh; height: 3.75vh; line-height: 3.75vh; font-size: 2.5vh; margin: 0; padding: 0;">
        <ons-icon icon="md-phone" style="font-size: 2.5vh; margin: 0; padding: 0; line-height: 0vh;"></ons-icon>
      </ons-fab>
    </div>
 
    <div v-else>
      <div style="text-align: center; font-size: 1.875vh; margin-top: 3.75vh; color: #000;">
        <p>Aktiver Anruf</p>
        <div v-if="callData.name !== null" style="text-align: center; font-size: 1.40625vh; margin-top: 0.46875vh; color: #000;">{{checkname(callData.name)}}</div>
        <div v-else-if="callData.name === null" style="text-align: center; font-size: 1.40625vh; margin-top: 0.46875vh; color: #000;">{{callData.caller}}</div>
        <div style="text-align: center; font-size: 1.875vh; margin-top:2.8125vh; color: #000;">{{time}}</div>
        <ons-fab v-if="mute == false" position="center" style="background-color: #A9A9A9; color: white; width: 3.75vh; height: 3.75vh; line-height: 3.75vh; font-size: 2.5vh; margin: 0; padding: 0; margin-top: 1vh;">
          <ons-icon icon="md-mic" style="font-size: 2.5vh; margin: 0; padding: 0; line-height: 0vh;"></ons-icon>
        </ons-fab>
        <ons-fab v-if="mute == true" position="center" style="background-color: #A9A9A9; color: white; width: 3.75vh; height: 3.75vh; line-height: 3.75vh; font-size: 2.5vh; margin: 0; padding: 0; margin-top: 1vh;">
          <ons-icon icon="md-mic-off" style="font-size: 2.5vh; margin: 0; padding: 0; line-height: 0vh;"></ons-icon>
        </ons-fab>
        <ons-fab v-on:click="hangupCall" position="bottom center" style="background-color: red; color: white; width: 3.75vh; height: 3.75vh; line-height: 3.75vh; font-size: 2.5vh; margin: 0; padding: 0;">
          <ons-icon icon="md-phone-end" style="font-size: 2.5vh; margin: 0; padding: 0; line-height: 0vh;"></ons-icon>
        </ons-fab>
      </div>
    </div>

    <audio v-if="playincomming" loop autoplay controls ref="media" :volume="1.0" currentTime="0" type="audio/ogg" preload="auto" :src="playSound('incomming')"/>
    <audio v-if="playoutgoing" loop autoplay controls ref="media2" :volume="0.1" currentTime="0" type="audio/ogg" preload="auto" :src="playSound('outcomming')"/>

  </v-ons-page>
</template>

<script>
import Apps from "../apps"
import Sounds from "../sounds"
import Ringtones from "../sounds/ringtones"
import Ringtones2 from "../sounds/ringtones2"
var timer;

export default Apps.register({
    name: "CallManageApp",
    data() {
        return {
            callData: [],
            accepted: false,
            ringtone: 0,
            date: null,
            sec: 0,
            timerRuns: false,
            time: "",
            clock: new Date(),
            name: "",
            mute: false,
            playincomming: false,
            playoutgoing: false,
        }
    },
    methods: {
        playSound(data){
            if(data == 'outcomming'){
                return Sounds.ringtoneOutcomming()
            }else if(data == 'incomming'){
                if(this.ringtone >= 9 ){
                    return Ringtones2.ringtone(this.ringtone)
                }
                else {
                    return Ringtones.ringtone(this.ringtone)
                }
            }
        },
        changemicmute(){
            if(this.mute == false){
                this.mute = true
            }else if(this.mute == true){
                this.mute = false
            }
            this.triggerServer("muteCall",this.mute)
        },
        cancelCall(data){
            this.clock = new Date()
            this.clock = this.clock + "%"
            var clock = this.clock.split(' ')
            this.clock = clock[4]
            if(this.time != ""){
                this.clock = this.clock + " (" + this.time + ")"
            }
            this.trigger("addCallToHistory",JSON.stringify({ contact: this.callData.name, number: this.callData.caller, time: this.clock, accepted: this.accepted, method: this.callData.method}))
            this.callData = JSON.parse(data)
        },
        setCallData(data, ringtoneId, sound) {
            this.mute = false
            this.callData = JSON.parse(data)
            this.ringtone = ringtoneId;

            if (this.callData.method == 'incoming') {
                if(sound == 'false'){
                    this.playincomming = true
                }
            }else if (this.callData.method == 'outgoing') {
                this.playoutgoing = true
            }
        },
        setTimer(){
            this.sec++

            var hrs = Math.floor(this.sec / 3600);
            var min = Math.floor((this.sec - (hrs * 3600)) / 60);
            var seconds = this.sec - (hrs * 3600) - (min * 60);
            seconds = Math.round(seconds * 100) / 100
           
            var result = (hrs < 10 ? "0" + hrs : hrs);
            result += ":" + (min < 10 ? "0" + min : min);
            result += ":" + (seconds < 10 ? "0" + seconds : seconds);

            this.time = result
        },
        declineCall() {
            this.triggerServer("callDeclined")
            this.playincomming = false
            this.playoutgoing = false
            clearInterval(timer)
        },
        declineCallSmartphone(){
            this.triggerServer("callDeclined")
            this.playincomming = false
            this.playoutgoing = false
        },
        acceptCallSmartphone(){
            if(this.callData.method == 'incoming'){
                this.triggerServer("callAccepted")
                this.playincomming = false
                this.playoutgoing = false
            }
        },
        acceptCall() {
            clearInterval(timer)
            this.accepted = true
            this.seconds = 0
            timer = setInterval(this.setTimer,1000)
            this.playincomming = false
            this.playoutgoing = false
        },
        hangupCall() {
            this.timerRuns = false
            this.triggerServer("callDeclined")
            this.playincomming = false
            this.playoutgoing = false
            clearInterval(timer)
        },
        checkname(name){
           if(name.indexOf('000FAV') >= 0){
              var contact = name.split('00FAV');
              return contact[1];
            }else{
                return name;
            }
        },
    },
    mounted() {
        if (!window.document.onkeyup) {
            window.document.onkeyup = this.onKeyUp           
        }
    }
})
</script>

<style lang="scss" scoped>
    audio{ 
        display:none;
    }
</style>