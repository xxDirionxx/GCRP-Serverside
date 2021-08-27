<template>
    <v-ons-page>
        <v-ons-toolbar>
            <div class="left">
                <v-ons-toolbar-button>
                    <v-ons-back-button></v-ons-back-button>
                </v-ons-toolbar-button>
            </div>
            <div class="center">Nachrichten</div>
        </v-ons-toolbar>
        <v-ons-list>
            <v-ons-list-header>Nachricht senden</v-ons-list-header>
            <v-ons-list-item modifier="longdivider">
              <div class="center">
                <input style="width:100%; margin-right: 10px;" placeholder="Nummer eingeben" v-model="receiver" />
              </div>
            </v-ons-list-item>
            <v-ons-list-item modifier="longdivider">
              <div class="center">
                <input style="width:100%; margin-right: 10px;" placeholder="Nachricht eingeben" v-model="message" />
              </div>
            </v-ons-list-item>
            <v-ons-list-item modifier="longdivider">
              <v-ons-button style="width:100%; margin-right: 10px; margin-top: -1px; background-color: #808080;" class="color-background blue" modifier="large" v-on:click="sendGPS()">Standort senden</v-ons-button>
            </v-ons-list-item>
            <v-ons-list-item modifier="longdivider">
              <v-ons-button style="width:100%; margin-right: 10px; margin-top: -1px; background-color: #808080;" class="color-background blue" modifier="large" v-on:click="sendContact(receiver)">Kontakt senden</v-ons-button>
            </v-ons-list-item>
            <v-ons-list-item modifier="longdivider">
              <v-ons-button style="width:100%; margin-right: 10px; margin-top: -1px;" class="color-background blue" modifier="large" v-on:click="sendMessage">Nachricht senden</v-ons-button>
            </v-ons-list-item>
            <audio v-for="x in sendCount" v-bind:key="x" volume="0.1" visibility="visible" currentTime="0" autoplay controls type="audio/ogg" preload="auto" :src="playSound('sendSMS')"/>
        </v-ons-list>
    </v-ons-page>
</template>

<script>
    import Apps from "../../apps"
    import MessengerApp from "./MessengerApp"
    import MessengerContactsApp from "./MessengerContactsApp"
    import Sounds from "../../sounds"

    export default Apps.register({
        name: "MessengerMessageApp",
        data() {
            return {
              receiver: null,
              message: "",
              charAmount: 0,
              newMessage: "",
              sendCount: 0
            }
        },
        methods: {
            sendMessage(){
                this.checkInput()
                this.triggerServer("sendPhoneMessage", this.receiver, this.message)
                this.sendCount = this.sendCount + 1
                this.pageStack.pop()
            },
            playSound(data){
              if(data == 'sendSMS'){
                  return Sounds.sendSMS()
              }
            },
            onKeyUp() {
                if (event.key == "Enter" || event.key == "enter") {
                    this.sendMessage()
                }
            },
            checkInput(){
              for (this.charAmount = 0; this.charAmount < this.message.length; this.charAmount++) { 
                if([' ','ä','ö','ü','ß','a',"b",'c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z','0','1','2','3','4','5','6','7','8','9',',','.','-','_','|','§','$','%','&','#',':','*','+','?','!','='].indexOf(this.message[this.charAmount].toLowerCase()) == -1){
                  this.newMessage += '�'
                } else {
                  this.newMessage += this.message[this.charAmount]
                }                  
              }
              this.message = ""
              this.message = this.newMessage
              this.newMessage = ""
            },
            sendContact(number){
              if(number == null) return
                this.pageStack.push({ extends: MessengerContactsApp, data() { return { number: number } } })
            },
            sendGPS(){
              if(this.receiver == null) return
                this.trigger("getLocation", JSON.stringify({ x:0, y:0 }))
            },
            setGPSdata(x, y){
                this.sendCount = this.sendCount + 1
                this.triggerServer("sendPhoneMessage", this.receiver, "GPS$$" + x + "$$" + y)
                this.pageStack.pop()
                this.pageStack.pop()
                this.pageStack.push(MessengerApp)
            }
        },
        mounted() {
            this.scrollToBottom()
            window.addEventListener('keyup', this.onKeyUp)
        },
        destroyed: function () {
            window.removeEventListener("keyup", this.onKeyUp)
        },
        props: ['pageStack']
    })
</script>

<style lang="scss" scoped>
    audio{ 
        display:none;
    }
</style>