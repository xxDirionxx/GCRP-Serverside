<template>
    <v-ons-page>
        <v-ons-toolbar>
            <div class="left">
                <v-ons-toolbar-button>
                    <v-ons-icon icon="md-arrow-back, material:md-arrow-back" size="material: 1.875vh" v-on:click="backBtn"></v-ons-icon>
                </v-ons-toolbar-button>
            </div>
            <div class="center">Chat</div>
            <div class="right">
                <v-ons-toolbar-button>
                    <v-ons-icon v-if="msgPartnerNumber" icon="md-plus, material:md-plus" size="material: 1.875vh" v-on:click="addUser(msgPartnerNumber)"></v-ons-icon>
                    <v-ons-icon icon="md-phone-in-talk, material:md-phone-in-talk" size="material: 1.875vh" v-on:click="callUser"></v-ons-icon>
                    <v-ons-icon icon="ion-trash-a, material:md-delete" size="material: 1.875vh" style="color: #B22222;" v-on:click="deleteChat"></v-ons-icon>
                </v-ons-toolbar-button>
            </div>
        </v-ons-toolbar>
        <v-ons-list id="messages">
            <div modifier="inset" v-for="(chat,index) in messages" :key="chat.id" v-bind:class="{'receiverClass': chat.receiver}" class="chatMessage">
                <v-ons-list-header :class="{'receiverClass': chat.receiver}">
                    <div class="senderName">{{ checkname(chat.sender) }}</div>
                    <div class="senderDate">{{ chat.date }}</div>
                    <div style="clear:both;"></div>
                </v-ons-list-header>
                <template v-if="chat.message.indexOf('GPS$$') >= 0">
                    <v-ons-list-item style="font-size: 1.275vh;"><p class="fas fa-map-marker-alt"></p>&nbsp;Mein Standort:</v-ons-list-item>
                    <v-ons-list-item style="font-size: 1.275vh;"><div class="mapimg" :style="mapCoordinates(chat.message)" v-on:click="readGPS(chat.message)"><p class="fas fa-map-marker-alt fa-2x naviCenter"/></div></v-ons-list-item>
                </template>
                <template v-else-if="chat.message.indexOf('CONTACT$$') >= 0">
                    <v-ons-list-item style="font-size: 1.275vh;"><p class="far fa-address-book"></p>&nbsp;Kontakt:</v-ons-list-item>
                    <v-ons-list-item style="font-size: 1.275vh;">
                        <h4 style="display: block; font-size: 1.875vh; margin: 0;">{{contact("NAME",chat.message)}}</h4>
                        <p style="display: block;">Telefon: {{contact("NUMBER",chat.message)}}
                            <button style="vertical-align: middle; color: #696969; margin-left: 1rem;" class="fas fa-phone fa-1x" v-on:click="call(chat.message)"><v-ons-icon icon="md-address-book" class="list-item__icon"></v-ons-icon></button>
                            <button style="vertical-align: middle; color: #696969;" class="far fa-comments fa-1x" v-on:click="sendsms(chat.message)"><v-ons-icon icon="md-address-book" class="list-item__icon"></v-ons-icon></button>
                            <!--<button style="vertical-align: middle; color: #696969;" class="far fa-save fa-1x" v-on:click="savecontact(chat.message)"><v-ons-icon icon="md-address-book" class="list-item__icon"></v-ons-icon></button>-->
                        </p>
                    </v-ons-list-item>
                </template>
                <template v-else-if="chat.message.indexOf('LOTTO$$') >= 0">
                    <v-ons-list-item style="font-size: 1.275vh;">Lottoschein:</v-ons-list-item>
                    <v-ons-list-item style="font-size: 1.275vh;">
                        <h4 style="display: block; font-size: 1.875vh; margin: 0;">{{contact("NAME",chat.message)}}</h4>
                        <p style="display: block; width: 100%">1. Glückszahl: {{trimlotto(chat.message,1)}}</p>
                        <p style="display: block; width: 100%">2. Glückszahl: {{trimlotto(chat.message,2)}}</p>
                        <p style="display: block; width: 100%">3. Glückszahl: {{trimlotto(chat.message,3)}}</p>
                    </v-ons-list-item>
                    <v-ons-list-item style="font-size: 1.275vh;">Ziehungsdatum: {{trimlotto(chat.message,5)}}</v-ons-list-item>
                    <v-ons-list-item style="font-size: 1.275vh;">Preis: {{trimlotto(chat.message,4)}}$</v-ons-list-item>
                </template>
                <template v-else-if="chat.message.indexOf('IMAGE$$') >= 0">
                    <img style="width: 15vh; height: 15vh; margin-left: 4vh;" :src="getimage(chat.message)" />
                </template>
                <template v-else>
                    <v-ons-list-item class="message" style="font-size: 1.275vh;"><div class="center messageFont" style="font-size: 1.275vh;" v-html="scanemoji(chat.message,index)"></div></v-ons-list-item>
                </template>
            </div>
        </v-ons-list>
        <v-ons-bottom-toolbar>
            <button style="margin-left: 0.6vh; vertical-align: middle;" class="selectemoji far fa-grin fa-1x" v-on:click="showemoji"><v-ons-icon icon="" class="list-item__icon"></v-ons-icon></button>
            <input class="messengerInput" placeholder="Nachricht" v-model="answer" />

            <!--<button style="vertical-align: middle; margin-left: -0.5vh; margin-right: 0.5vh;" class="selectemoji far fa-image fa-1x" ><input style="vertical-align: middle; opacity:0; width: 100% height: 100%; margin-left: -3vh; padding-right: 2.5vh" class="selectemoji fas fa-map-marker-alt fa-1x" type="file" @change="onFileChange"></button>-->
            <button style="vertical-align: middle;" class="selectemoji fas fa-map-marker-alt fa-1x" v-on:click="sendGPS"><v-ons-icon icon="md-map-marker" class="list-item__icon"></v-ons-icon></button>
            <button style="vertical-align: middle;" class="selectemoji far fa-address-book fa-1x" v-on:click="sendContact(msgPartnerNumber)"><v-ons-icon icon="md-address-book" class="list-item__icon"></v-ons-icon></button>
            <button style="vertical-align: middle; color: #0076ff;" v-on:click="sendMessage"><v-ons-icon icon="md-mail-send" class="list-item__icon"></v-ons-icon></button>
            <div v-if="emotevisible == true && emojikat == 'normal'" id="emoji01" style="width: 100%; height: 3vh;">
                <div class="emoji" style="margin-left: 0.3vh; background-position: 0% 0%;" v-on:click="sendemoji(':uff')"></div>
                <div class="emoji" style="background-position: 11% 0%;" v-on:click="sendemoji(':kiss')"></div>
                <div class="emoji" style="background-position: 22% 0%;" v-on:click="sendemoji(':smile')"></div>
                <div class="emoji" style="background-position: 33% 0%;" v-on:click="sendemoji(':crazy')"></div>
                <div class="emoji" style="background-position: 44% 0%;" v-on:click="sendemoji(':shy')"></div>
                <div class="emoji" style="background-position: 55% 0%;" v-on:click="sendemoji(':shock')"></div>
                <div class="emoji" style="background-position: 66% 0%;" v-on:click="sendemoji(':blink')"></div>
                <div class="emoji" style="background-position: 77% 0%;" v-on:click="sendemoji(':love')"></div>
                <div class="emoji" style="background-position: 89% 0%;" v-on:click="sendemoji(':sad')"></div>
            </div>

            <div v-if="emotevisible == true && emojikat == 'normal'" id="emoji02" style="width: 100%; height: 3vh;">
                <div class="emoji" style="margin-left: 0.3vh; background-position: 0% 11%;" v-on:click="sendemoji(':jane')"></div>
                <div class="emoji" style="background-position: 11% 11%;" v-on:click="sendemoji(':sleep')"></div>
                <div class="emoji" style="background-position: 22% 11%;" v-on:click="sendemoji(':cry')"></div>
                <div class="emoji" style="background-position: 33% 11%;" v-on:click="sendemoji(':lol')"></div>
                <div class="emoji" style="background-position: 44% 11%;" v-on:click="sendemoji(':happy')"></div>
                <div class="emoji" style="background-position: 55% 11%;" v-on:click="sendemoji(':mimimi')"></div>
                <div class="emoji" style="background-position: 66% 11%;" v-on:click="sendemoji(':fearful')"></div>
                <div class="emoji" style="background-position: 77% 11%;" v-on:click="sendemoji(':scream')"></div>
                <div class="emoji" style="background-position: 89% 11%;" v-on:click="sendemoji(':yummy')"></div>
            </div>

            <div v-if="emotevisible == true && emojikat == 'normal'" id="emoji02" style="width: 100%; height: 3vh;">
                <div class="emoji" style="margin-left: 0.3vh; background-position: 11% 22%;" v-on:click="sendemoji(':think')"></div>
                <div class="emoji" style="background-position: 66% 22%;" v-on:click="sendemoji(':roll')"></div>
                <div class="emoji" style="background-position: 77% 22%;" v-on:click="sendemoji(':rage')"></div>
                <div class="emoji" style="background-position: 89% 22%;" v-on:click="sendemoji(':kotz')"></div>
                <div class="emoji" style="background-position: 100% 0%;" v-on:click="sendemoji(':bored')"></div>
                <div class="emoji" style="background-position: 100% 11%;" v-on:click="sendemoji(':cool')"></div>
                <div class="emoji" style="background-position: 11% 33%;" v-on:click="sendemoji(':mute')"></div>
                <div class="emoji" style="background-position: 22% 33%;" v-on:click="sendemoji(':devil')"></div>
                <div class="emoji" style="background-position: 77% 33%;" v-on:click="sendemoji(':nerd')"></div>
            </div>

            <div v-if="emotevisible == true && emojikat == 'normal'" id="emoji02" style="width: 100%; height: 3vh;">
                <div class="emoji" style="margin-left: 0.3vh; background-position: 89% 33%;" v-on:click="sendemoji(':laura')"></div>
                <div class="emoji" style="background-position: 22% 44%;" v-on:click="sendemoji(':rofl')"></div>
                <div class="emoji" style="background-position: 89% 44%;" v-on:click="sendemoji(':smirking')"></div>
                <div class="emoji" style="background-position: 33% 55%;" v-on:click="sendemoji(':unschuldig')"></div>
            </div>

            <div v-if="emotevisible == true && emojikat == 'haende'" id="emoji02" style="width: 100%; height: 3vh;">
                <div class="emoji" style="margin-left: 0.3vh; background-position: 66% 33%;" v-on:click="sendemoji(':peace')"></div>
                <div class="emoji" style="background-position: 22% 22%;" v-on:click="sendemoji(':like')"></div>
                <div class="emoji" style="background-position: 33% 22%;" v-on:click="sendemoji(':loch')"></div>
                <div class="emoji" style="background-position: 33% 33%;" v-on:click="sendemoji(':fuck')"></div>
                <div class="emoji" style="background-position: 0% 44%;" v-on:click="sendemoji(':finger')"></div>
                <div class="emoji" style="background-position: 11% 44%;" v-on:click="sendemoji(':faust')"></div>
                <div class="emoji" style="background-position: 55% 55%;" v-on:click="sendemoji(':wave')"></div>
            </div>

            <div v-if="emotevisible == true && emojikat == 'haende'" id="emoji02" style="width: 100%; height: 3vh;">
                
            </div>

            <div v-if="emotevisible == true && emojikat == 'haende'" id="emoji02" style="width: 100%; height: 3vh;">
                
            </div>

            <div v-if="emotevisible == true && emojikat == 'haende'" id="emoji02" style="width: 100%; height: 3vh;">
                
            </div>

            <div v-if="emotevisible == true && emojikat == 'fraktionen'" id="emoji02" style="width: 100%; height: 3vh;">
                <div class="emoji" style="margin-left: 0.3vh; background-position: 33% 66%;" v-on:click="sendemoji(':lcn')"></div>
                <div class="emoji" style="background-position: 0% 66%;" v-on:click="sendemoji(':ballas')"></div>
                <div class="emoji" style="background-position: 11% 66%;" v-on:click="sendemoji(':lost')"></div>
                <div class="emoji" style="background-position: 22% 66%;" v-on:click="sendemoji(':vagos')"></div>
                <div class="emoji" style="background-position: 44% 66%;" v-on:click="sendemoji(':yakuza')"></div>
                <div class="emoji" style="background-position: 55% 66%;" v-on:click="sendemoji(':grove')"></div>
                <div class="emoji" style="background-position: 66% 66%;" v-on:click="sendemoji(':triaden')"></div>
                <div class="emoji" style="background-position: 77% 66%;" v-on:click="sendemoji(':mg13')"></div>
                <div class="emoji" style="background-position: 89% 66%;" v-on:click="sendemoji(':brigada')"></div>
            </div>

            <div v-if="emotevisible == true && emojikat == 'fraktionen'" id="emoji02" style="width: 100%; height: 3vh;">
                <div class="emoji" style="margin-left: 0.3vh; background-position: 22% 77%;" v-on:click="sendemoji(':bratwa')"></div>
                <div class="emoji" style="background-position: 100% 66%;" v-on:click="sendemoji(':midnight')"></div>
                <div class="emoji" style="background-position: 33% 77%;" v-on:click="sendemoji(':lsmc')"></div>
                <div class="emoji" style="background-position: 55% 77%;" v-on:click="sendemoji(':lspd')"></div>
                <div class="emoji" style="background-position: 66% 77%;" v-on:click="sendemoji(':dmv')"></div>
                <div class="emoji" style="background-position: 77% 77%;" v-on:click="sendemoji(':dpos')"></div>
                <div class="emoji" style="background-position: 89% 77%;" v-on:click="sendemoji(':army')"></div>
                <div class="emoji" style="background-position: 0% 88%;" v-on:click="sendemoji(':fib')"></div>
                <div class="emoji" style="background-position: 11% 88%;" v-on:click="sendemoji(':weazel')"></div>
            </div>

            <div v-if="emotevisible == true && emojikat == 'fraktionen'" id="emoji02" style="width: 100%; height: 3vh;">
                <div class="emoji" style="margin-left: 0.3vh; background-position: 22% 88%;" v-on:click="sendemoji(':gov')"></div>
                <div class="emoji" style="background-position: 33% 88%;" v-on:click="sendemoji(':coastguard')"></div>
                <div class="emoji" style="background-position: 44% 88%;" v-on:click="sendemoji(':airforce')"></div>
            </div>

            <div v-if="emotevisible == true && emojikat == 'fraktionen'" id="emoji02" style="width: 100%; height: 3vh;">

            </div>

            <div v-if="emotevisible == true && emojikat == 'gegenstaende'" id="emoji02" style="width: 100%; height: 3vh;">
                <div class="emoji" style="margin-left: 0.3vh; background-position: 100% 33%;" v-on:click="sendemoji(':water')"></div>
                <div class="emoji" style="background-position: 44% 33%;" v-on:click="sendemoji(':beer')"></div>
                <div class="emoji" style="background-position: 44% 44%;" v-on:click="sendemoji(':ivan')"></div>
                <div class="emoji" style="background-position: 55% 44%;" v-on:click="sendemoji(':roxy')"></div>
                <div class="emoji" style="background-position: 100% 44%;" v-on:click="sendemoji(':teddy')"></div>
                <div class="emoji" style="background-position: 0% 55%;" v-on:click="sendemoji(':aubergine')"></div>
                <div class="emoji" style="background-position: 22% 55%;" v-on:click="sendemoji(':peach')"></div>
                <div class="emoji" style="background-position: 44% 55%;" v-on:click="sendemoji(':frog')"></div>
                <div class="emoji" style="background-position: 66% 55%;" v-on:click="sendemoji(':gun')"></div>
            </div>

            <div v-if="emotevisible == true && emojikat == 'gegenstaende'" id="emoji02" style="width: 100%; height: 3vh;">
                <div class="emoji" style="margin-left: 0.3vh; background-position: 77% 55%;" v-on:click="sendemoji(':ephi')"></div>
                <div class="emoji" style="background-position: 89% 55%;" v-on:click="sendemoji(':meth')"></div>
                <div class="emoji" style="background-position: 100% 55%;" v-on:click="sendemoji(':dildo')"></div>
                <div class="emoji" style="background-position: 0% 77%;" v-on:click="sendemoji(':weed')"></div>
                <div class="emoji" style="background-position: 11% 77%;" v-on:click="sendemoji(':joint')"></div>
                <div class="emoji" style="background-position: 44% 77%;" v-on:click="sendemoji(':diamant')"></div>
            </div>

            <div v-if="emotevisible == true && emojikat == 'gegenstaende'" id="emoji02" style="width: 100%; height: 3vh;">

            </div>

            <div v-if="emotevisible == true && emojikat == 'gegenstaende'" id="emoji02" style="width: 100%; height: 3vh;">

            </div>

            <div v-if="emotevisible == true && emojikat == 'sonstiges'" id="emoji02" style="width: 100%; height: 3vh;">
                <div class="emoji" style="margin-left: 0.3vh; background-position: 0% 22%;" v-on:click="sendemoji(':chickie')"></div>
                <div class="emoji" style="background-position: 44% 22%;" v-on:click="sendemoji(':heart')"></div>
                <div class="emoji" style="background-position: 55% 22%;" v-on:click="sendemoji(':affe')"></div>
                <div class="emoji" style="background-position: 0% 33%;" v-on:click="sendemoji(':gtmp')"></div>
                <div class="emoji" style="background-position: 55% 33%;" v-on:click="sendemoji(':wollow')"></div>
                <div class="emoji" style="background-position: 100% 22%;" v-on:click="sendemoji(':facepalm')"></div>
                <div class="emoji" style="background-position: 33% 44%;" v-on:click="sendemoji(':hugh')"></div>
                <div class="emoji" style="background-position: 66% 44%;" v-on:click="sendemoji(':audreyshin')"></div>
                <div class="emoji" style="background-position: 77% 44%;" v-on:click="sendemoji(':100')"></div>
            </div>

            <div v-if="emotevisible == true && emojikat == 'sonstiges'" id="emoji02" style="width: 100%; height: 3vh;">
                <div class="emoji" style="margin-left: 0.3vh; background-position: 11% 55%;" v-on:click="sendemoji(':augemachen')"></div>
                <div class="emoji" style="background-position: 100% 77%;" v-on:click="sendemoji(':dollar')"></div>
                <div class="emoji" style="background-position: 55% 88%;" v-on:click="sendemoji(':farid')"></div>
            </div>

            <div v-if="emotevisible == true && emojikat == 'sonstiges'" id="emoji02" style="width: 100%; height: 3vh;">

            </div>

            <div v-if="emotevisible == true && emojikat == 'sonstiges'" id="emoji02" style="width: 100%; height: 3vh;">

            </div>

            <div v-if="emotevisible == true" style="width: 100%; height: 3vh;">
                <div class="selectemoji far fa-grin fa-1x" style="margin-left: 2.3vh;" v-on:click="selectemojikat('normal')"></div>
                <div class="selectemoji far fa-hand-peace fa-1x" style="margin-left: 2.3vh;" v-on:click="selectemojikat('haende')"></div>
                <div class="selectemoji far fa-building fa-1x" style="margin-left: 2.3vh;" v-on:click="selectemojikat('fraktionen')"></div>
                <div class="selectemoji far fa-life-ring fa-1x" style="margin-left: 2.3vh;" v-on:click="selectemojikat('gegenstaende')"></div>
                <div class="selectemoji far fa-question-circle fa-1x" style="margin-left: 2.3vh;" v-on:click="selectemojikat('sonstiges')"></div>
            </div>
        </v-ons-bottom-toolbar>
        <audio v-for="x in sendCount" v-bind:key="x" volume="0.1" visibility="visible" currentTime="0" autoplay controls type="audio/ogg" preload="auto" :src="playSound('sendSMS')"/>
    </v-ons-page>
</template>

<script>
    import Apps from "../../apps"
    import MessengerContactsApp from "./MessengerContactsApp"
    import MessageOverviewApp from "./MessageOverviewApp"
    import ContactAdd from "../contacts/ContactsAdd"
    import MessengerApp from "./MessengerApp"
    import Sounds from "../../sounds"
    import Notify from "../../hud/PlayerNotification"

    export default Apps.register({
        name: "MessengerOverviewApp",
        data() {
            return {
                messages: [],
                msgId: null,
                msgPartner: null,
                msgPartnerNumber: null,
                answer: "",
                charAmount: 0,
                newMessage: "",
                myphone: "",
                emotevisible: false,
                encodedHTML: "",
                emojikat: "normal",
                sendCount: 0,
                image: '',
                lotto: false,
                frakTeam: 0,
                frakRank: 0,
                firstname: "",
                lastname: ""
            }
        },
        methods: {
            onFileChange(e) {
                var files = e.target.files || e.dataTransfer.files;
                if (!files.length)
                    return;
                this.image = this.createImage(files[0]);
                this.sendimage(this.image)
            },
            createImage(file) {
                if(file.size <= 512*512){
                    var reader = new FileReader();

                    reader.onload = (e) => {
                        //vm.image = e.target.result;
                        this.sendimage(e.target.result)
                    }
                    reader.readAsDataURL(file);
                }else{
                    Notify.pushPlayerNotification("Das Image darf max. 512 x 512 Pixel groß sein.",3000,false)
                }
            },
            sendimage(img){
                var imagedata = img.split(',');
                this.triggerServer("sendPhoneMessage", this.msgPartnerNumber, "IMAGE$$" + imagedata[1]);
            },
            getimage(imagemessage){
                var imagedata = imagemessage.split('$$');
                return "data:image/jpeg;base64," + imagedata[1]
            },
            sendMessage() {
                if (this.answer == "" || this.answer.trim() == "") return
                var messageDate = this.answer.split('$$');
                if(messageDate[0] == 'IMAGE'){
                    this.triggerServer("sendPhoneMessage", this.msgPartnerNumber, this.answer)
                }else{
                    this.checkInput()
                    this.triggerServer("sendPhoneMessage", this.msgPartnerNumber, this.answer)
                }
                this.sendCount = this.sendCount + 1
                this.answer = ""
            },
            playSound(data){
              if(data == 'sendSMS'){
                  return Sounds.sendSMS()
              }
            },
            setNumber(number, team, rank, pfirstname, plastname){
                this.myphone = number.toString()
                this.frakTeam = team;
                this.frakRank = rank;
                this.firstname = pfirstname;
                this.lastname = plastname;
            },
            sendMessageContact(contact) {
                this.triggerServer("sendPhoneMessage", this.msgPartnerNumber, contact)
            },
            sendGPS(){
                this.trigger("getLocation", JSON.stringify({ x:0, y:0 }))
            },
            setGPSdata(x, y){
                this.sendCount = this.sendCount + 1
                this.triggerServer("sendPhoneMessage", this.msgPartnerNumber, "GPS$$" + x + "$$" + y)
            },
            sendContact(number){
                this.pageStack.push({ extends: MessengerContactsApp, data() { return { number: number } } })
            },
            callUser() {
                this.triggerServer("callUserPhone", this.msgPartnerNumber)
            },
            addUser(number){
                this.pageStack.push({ extends: ContactAdd, data() { return {number: number } } })
            },
            trimlotto(message, number){
                var schein = message.split('$$');
                this.lotto = true
                return schein[1 + number]
            },
            readGPS(gpsdata){
                var gps = gpsdata.split('$$');
                var gpsX = parseInt(gps[1]);
                var gpsY = parseInt(gps[2]);
                this.trigger("setGpsCoordinates", JSON.stringify({ x:gpsX, y:gpsY }))
            },
            deleteChat() {
                this.triggerServer("deletePhoneChat", this.msgId)
                this.pageStack.splice(1, this.pageStack.length - 1)
            },
            backBtn() {
                this.pageStack.pop()
                this.pageStack.pop()
                this.pageStack.push(MessengerApp)
            },
            updateChat(newMessage) {
                if(newMessage == "" || newMessage == undefined || newMessage == null) return
                if(JSON.parse(newMessage).sender == this.msgPartner || JSON.parse(newMessage).sender == this.msgPartnerNumber || JSON.parse(newMessage).sender == "Ich" || JSON.parse(newMessage).sender == this.myphone){
                    this.messages.push(JSON.parse(newMessage))
                    this.scrollToBottom()
                }
            },
            onKeyUp() {
                if (event.key == "Enter" || event.key == "enter") {
                    this.sendMessage()
                }
            },
            scrollToBottom() {
                setTimeout(() => {
                    const messages = document.getElementById("messages").childNodes
                    const length = this.messages.length - 1
                    messages[length].scrollIntoView(false)
                    console.log("scroll", length);
                }, 500)
            },
            contact(modus, contactdata){
                var contact = contactdata.split('$$');
                if(modus == "NAME"){
                    return contact[1];
                }else if(modus == "NUMBER"){
                    return contact[2];
                }
            },
            call(contactdata){
                var contact = contactdata.split('$$');
                this.triggerServer("callUserPhone", contact[2])
            },
            sendsms(contactdata){
                var contact = contactdata.split('$$');
                this.pageStack.push({ extends: MessageOverviewApp, data() { return { msgPartner: contact[1], msgPartnerNumber: contact[2] } } })
            },
            savecontact(contactdata){
                var contact = contactdata.split('$$');
                this.pageStack.push({ extends: ContactAdd, data() { return { name: contact[1],number: contact[2] } } })
            },
            mapCoordinates(gpsdata){
                var calcHelper;
                var gps = gpsdata.split('$$');
                var corX = 45.7;
                if(gps[1] > 0){
                    corX = corX + (gps[1] / 120);
                }else{
                    calcHelper = gps[1] / 120;
                    corX = corX - (calcHelper * -1);
                }
                var corY = 67.6;
                if(gps[2] > 0){
                    corY = corY - (gps[2] / 120);
                }else{
                    calcHelper = gps[2] / 120;
                    corY = corY + (calcHelper * -1);
                }
                return {
                    backgroundPosition: (corX + '%' + corY + '%')
                };
            },
            checkInput(){
              for (this.charAmount = 0; this.charAmount < this.answer.length; this.charAmount++) { 
                if([' ','ä','ö','ü','ß','a',"b",'c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z','0','1','2','3','4','5','6','7','8','9',',','.','-','_','|','§','$','%','&','#',':','*','+','?','!','='].indexOf(this.answer[this.charAmount].toLowerCase()) == -1){
                  this.newMessage += '�'
                } else {
                  this.newMessage += this.answer[this.charAmount]
                }                  
              }
              this.answer = ""
              this.answer = this.newMessage
              this.newMessage = ""
            },
            checkname(name){
                if(name.indexOf('000FAV') >= 0){
                    var contact = name.split('00FAV');
                    return contact[1];
                }else{
                    return name;
                }
            },
            showemoji(){
                if(this.emotevisible == false){
                    this.emotevisible = true
                }else{
                    this.emotevisible = false
                    this.emojikat = "normal"
                }
            },
            sendemoji(title){
                this.answer = this.answer + " " + title + " ";
                this.emotevisible = false
                this.emojikat = "normal"
            },
            selectemojikat(name){
                this.emojikat = name
            },
            createemojihtml(wert){
                const addhtml = '<div class="emodji" style="display: inline-block; background-color: #125485; width: 2.5vh; height: 2.5vh; ' + "background: url('/img/allemoji.f66a5798.png')" + ' no-repeat; background-size: 1000%;background-position: '
                const addhtml2 = ';"></div>'
                return addhtml + wert + addhtml2
            },
            scanemoji(message,index){
                console.log("check: " + index)
                const contentWithEmoji = message.replace(":uff", this.createemojihtml('0% 0%'))
                    .replace(":kiss", this.createemojihtml('11% 0%'))
                    .replace(":smile", this.createemojihtml('22% 0%')) 
                    .replace(":crazy", this.createemojihtml('33% 0%'))
                    .replace(":shy", this.createemojihtml('44% 0%'))
                    .replace(":shock", this.createemojihtml('55% 0%'))   
                    .replace(":blink", this.createemojihtml('66% 0%'))   
                    .replace(":love", this.createemojihtml('77% 0%'))   
                    .replace(":sad", this.createemojihtml('89% 0%'))   

                    .replace(":jane", this.createemojihtml('0% 11%'))  
                    .replace(":sleep", this.createemojihtml('11% 11%'))   
                    .replace(":cry", this.createemojihtml('22% 11%'))   
                    .replace(":lol", this.createemojihtml('33% 11%'))   
                    .replace(":happy", this.createemojihtml('44% 11%'))   
                    .replace(":mimimi", this.createemojihtml('55% 11%'))   
                    .replace(":fearful", this.createemojihtml('66% 11%'))   
                    .replace(":scream", this.createemojihtml('77% 11%'))   
                    .replace(":yummy", this.createemojihtml('89% 11%'))   

                    .replace(":chickie", this.createemojihtml('0% 22%'))  
                    .replace(":think", this.createemojihtml('11% 22%'))   
                    .replace(":like", this.createemojihtml('22% 22%'))   
                    .replace(":loch", this.createemojihtml('33% 22%'))   
                    .replace(":heart", this.createemojihtml('44% 22%'))   
                    .replace(":affe", this.createemojihtml('55% 22%'))   
                    .replace(":roll", this.createemojihtml('66% 22%'))   
                    .replace(":rage", this.createemojihtml('77% 22%'))   
                    .replace(":kotz", this.createemojihtml('89% 22%'))   

                    .replace(":bored", this.createemojihtml('100% 0%'))
                    .replace(":cool", this.createemojihtml('100% 11%'))   
                    .replace(":facepalm", this.createemojihtml('100% 22%'))   
                    .replace(":gtmp", this.createemojihtml('0% 33%'))
                    .replace(":mute", this.createemojihtml('11% 33%'))   
                    .replace(":devil", this.createemojihtml('22% 33%'))   
                    .replace(":fuck", this.createemojihtml('33% 33%'))   
                    .replace(":beer", this.createemojihtml('44% 33%'))   
                    .replace(":wollow", this.createemojihtml('55% 33%'))

                    .replace(":peace", this.createemojihtml('66% 33%'))
                    .replace(":nerd", this.createemojihtml('77% 33%'))   
                    .replace(":laura", this.createemojihtml('89% 33%'))   
                    .replace(":water", this.createemojihtml('100% 33%'))
                    .replace(":finger", this.createemojihtml('0% 44%'))   
                    .replace(":faust", this.createemojihtml('11% 44%'))   
                    .replace(":rofl", this.createemojihtml('22% 44%'))   
                    .replace(":hugh", this.createemojihtml('33% 44%'))   
                    .replace(":ivan", this.createemojihtml('44% 44%'))

                    .replace(":roxy", this.createemojihtml('55% 44%'))
                    .replace(":audreyshin", this.createemojihtml('66% 44%'))   
                    .replace(":100", this.createemojihtml('77% 44%'))   
                    .replace(":smirking", this.createemojihtml('89% 44%'))
                    .replace(":teddy", this.createemojihtml('100% 44%'))   
                    .replace(":aubergine", this.createemojihtml('0% 55%'))   
                    .replace(":augemachen", this.createemojihtml('11% 55%'))   
                    .replace(":peach", this.createemojihtml('22% 55%'))   
                    .replace(":unschuldig", this.createemojihtml('33% 55%'))

                    .replace(":frog", this.createemojihtml('44% 55%'))
                    .replace(":wave", this.createemojihtml('55% 55%'))   
                    .replace(":gun", this.createemojihtml('66% 55%'))   
                    .replace(":ephi", this.createemojihtml('77% 55%'))
                    .replace(":meth", this.createemojihtml('89% 55%'))   
                    .replace(":dildo", this.createemojihtml('100% 55%'))   
                    .replace(":ballas", this.createemojihtml('0% 66%'))   
                    .replace(":lost", this.createemojihtml('11% 66%'))   
                    .replace(":vagos", this.createemojihtml('22% 66%'))

                    .replace(":lcn", this.createemojihtml('33% 66%'))
                    .replace(":yakuza", this.createemojihtml('44% 66%'))   
                    .replace(":grove", this.createemojihtml('55% 66%'))   
                    .replace(":triaden", this.createemojihtml('66% 66%'))
                    .replace(":mg13", this.createemojihtml('77% 66%'))   
                    .replace(":brigada", this.createemojihtml('89% 66%'))   
                    .replace(":midnight", this.createemojihtml('100% 66%'))   
                    .replace(":weed", this.createemojihtml('0% 77%'))   
                    .replace(":joint", this.createemojihtml('11% 77%'))

                    .replace(":bratwa", this.createemojihtml('22% 77%'))
                    .replace(":lsmc", this.createemojihtml('33% 77%'))   
                    .replace(":diamant", this.createemojihtml('44% 77%'))   
                    .replace(":lspd", this.createemojihtml('55% 77%'))
                    .replace(":dmv", this.createemojihtml('66% 77%'))   
                    .replace(":dpos", this.createemojihtml('77% 77%'))   
                    .replace(":army", this.createemojihtml('89% 77%'))   
                    .replace(":dollar", this.createemojihtml('100% 77%'))   
                    .replace(":fib", this.createemojihtml('0% 88%'))

                    .replace(":weazel", this.createemojihtml('11% 88%'))
                    .replace(":gov", this.createemojihtml('22% 88%'))
                    .replace(":coastguard", this.createemojihtml('33% 88%'))
                    .replace(":airforce", this.createemojihtml('44% 88%'))
                    .replace(":farid", this.createemojihtml('55% 88%'))

                    .replace(":uff", this.createemojihtml('0% 0%'))
                    .replace(":kiss", this.createemojihtml('11% 0%'))
                    .replace(":smile", this.createemojihtml('22% 0%')) 
                    .replace(":crazy", this.createemojihtml('33% 0%'))
                    .replace(":shy", this.createemojihtml('44% 0%'))
                    .replace(":shock", this.createemojihtml('55% 0%'))   
                    .replace(":blink", this.createemojihtml('66% 0%'))   
                    .replace(":love", this.createemojihtml('77% 0%'))   
                    .replace(":sad", this.createemojihtml('89% 0%'))   

                    .replace(":jane", this.createemojihtml('0% 11%'))  
                    .replace(":sleep", this.createemojihtml('11% 11%'))   
                    .replace(":cry", this.createemojihtml('22% 11%'))   
                    .replace(":lol", this.createemojihtml('33% 11%'))   
                    .replace(":happy", this.createemojihtml('44% 11%'))   
                    .replace(":mimimi", this.createemojihtml('55% 11%'))   
                    .replace(":fearful", this.createemojihtml('66% 11%'))   
                    .replace(":scream", this.createemojihtml('77% 11%'))   
                    .replace(":yummy", this.createemojihtml('89% 11%'))   

                    .replace(":chickie", this.createemojihtml('0% 22%'))  
                    .replace(":think", this.createemojihtml('11% 22%'))   
                    .replace(":like", this.createemojihtml('22% 22%'))   
                    .replace(":loch", this.createemojihtml('33% 22%'))   
                    .replace(":heart", this.createemojihtml('44% 22%'))   
                    .replace(":affe", this.createemojihtml('55% 22%'))   
                    .replace(":roll", this.createemojihtml('66% 22%'))   
                    .replace(":rage", this.createemojihtml('77% 22%'))   
                    .replace(":kotz", this.createemojihtml('89% 22%'))   

                    .replace(":bored", this.createemojihtml('100% 0%'))
                    .replace(":cool", this.createemojihtml('100% 11%'))   
                    .replace(":facepalm", this.createemojihtml('100% 22%'))   
                    .replace(":gtmp", this.createemojihtml('0% 33%'))
                    .replace(":mute", this.createemojihtml('11% 33%'))   
                    .replace(":devil", this.createemojihtml('22% 33%'))   
                    .replace(":fuck", this.createemojihtml('33% 33%'))   
                    .replace(":beer", this.createemojihtml('44% 33%'))   
                    .replace(":wollow", this.createemojihtml('55% 33%'))

                    .replace(":peace", this.createemojihtml('66% 33%'))
                    .replace(":nerd", this.createemojihtml('77% 33%'))   
                    .replace(":laura", this.createemojihtml('89% 33%'))   
                    .replace(":water", this.createemojihtml('100% 33%'))
                    .replace(":finger", this.createemojihtml('0% 44%'))   
                    .replace(":faust", this.createemojihtml('11% 44%'))   
                    .replace(":rofl", this.createemojihtml('22% 44%'))   
                    .replace(":hugh", this.createemojihtml('33% 44%'))   
                    .replace(":ivan", this.createemojihtml('44% 44%'))

                    .replace(":roxy", this.createemojihtml('55% 44%'))
                    .replace(":audreyshin", this.createemojihtml('66% 44%'))   
                    .replace(":100", this.createemojihtml('77% 44%'))   
                    .replace(":smirking", this.createemojihtml('89% 44%'))
                    .replace(":teddy", this.createemojihtml('100% 44%'))   
                    .replace(":aubergine", this.createemojihtml('0% 55%'))   
                    .replace(":augemachen", this.createemojihtml('11% 55%'))   
                    .replace(":peach", this.createemojihtml('22% 55%'))   
                    .replace(":unschuldig", this.createemojihtml('33% 55%'))

                    .replace(":frog", this.createemojihtml('44% 55%'))
                    .replace(":wave", this.createemojihtml('55% 55%'))   
                    .replace(":gun", this.createemojihtml('66% 55%'))   
                    .replace(":ephi", this.createemojihtml('77% 55%'))
                    .replace(":meth", this.createemojihtml('89% 55%'))   
                    .replace(":dildo", this.createemojihtml('100% 55%'))   
                    .replace(":ballas", this.createemojihtml('0% 66%'))   
                    .replace(":lost", this.createemojihtml('11% 66%'))   
                    .replace(":vagos", this.createemojihtml('22% 66%'))

                    .replace(":lcn", this.createemojihtml('33% 66%'))
                    .replace(":yakuza", this.createemojihtml('44% 66%'))   
                    .replace(":grove", this.createemojihtml('55% 66%'))   
                    .replace(":triaden", this.createemojihtml('66% 66%'))
                    .replace(":mg13", this.createemojihtml('77% 66%'))   
                    .replace(":brigada", this.createemojihtml('89% 66%'))   
                    .replace(":midnight", this.createemojihtml('100% 66%'))   
                    .replace(":weed", this.createemojihtml('0% 77%'))   
                    .replace(":joint", this.createemojihtml('11% 77%'))

                    .replace(":bratwa", this.createemojihtml('22% 77%'))
                    .replace(":lsmc", this.createemojihtml('33% 77%'))   
                    .replace(":diamant", this.createemojihtml('44% 77%'))   
                    .replace(":lspd", this.createemojihtml('55% 77%'))
                    .replace(":dmv", this.createemojihtml('66% 77%'))   
                    .replace(":dpos", this.createemojihtml('77% 77%'))   
                    .replace(":army", this.createemojihtml('89% 77%'))   
                    .replace(":dollar", this.createemojihtml('100% 77%'))   
                    .replace(":fib", this.createemojihtml('0% 88%'))

                    .replace(":weazel", this.createemojihtml('11% 88%'))
                    .replace(":gov", this.createemojihtml('22% 88%'))
                    .replace(":coastguard", this.createemojihtml('33% 88%'))
                    .replace(":airforce", this.createemojihtml('44% 88%'))
                    .replace(":farid", this.createemojihtml('55% 88%'))

                    .replace(":uff", this.createemojihtml('0% 0%'))
                    .replace(":kiss", this.createemojihtml('11% 0%'))
                    .replace(":smile", this.createemojihtml('22% 0%')) 
                    .replace(":crazy", this.createemojihtml('33% 0%'))
                    .replace(":shy", this.createemojihtml('44% 0%'))
                    .replace(":shock", this.createemojihtml('55% 0%'))   
                    .replace(":blink", this.createemojihtml('66% 0%'))   
                    .replace(":love", this.createemojihtml('77% 0%'))   
                    .replace(":sad", this.createemojihtml('89% 0%'))   

                    .replace(":jane", this.createemojihtml('0% 11%'))  
                    .replace(":sleep", this.createemojihtml('11% 11%'))   
                    .replace(":cry", this.createemojihtml('22% 11%'))   
                    .replace(":lol", this.createemojihtml('33% 11%'))   
                    .replace(":happy", this.createemojihtml('44% 11%'))   
                    .replace(":mimimi", this.createemojihtml('55% 11%'))   
                    .replace(":fearful", this.createemojihtml('66% 11%'))   
                    .replace(":scream", this.createemojihtml('77% 11%'))   
                    .replace(":yummy", this.createemojihtml('89% 11%'))   

                    .replace(":chickie", this.createemojihtml('0% 22%'))  
                    .replace(":think", this.createemojihtml('11% 22%'))   
                    .replace(":like", this.createemojihtml('22% 22%'))   
                    .replace(":loch", this.createemojihtml('33% 22%'))   
                    .replace(":heart", this.createemojihtml('44% 22%'))   
                    .replace(":affe", this.createemojihtml('55% 22%'))   
                    .replace(":roll", this.createemojihtml('66% 22%'))   
                    .replace(":rage", this.createemojihtml('77% 22%'))   
                    .replace(":kotz", this.createemojihtml('89% 22%'))   

                    .replace(":bored", this.createemojihtml('100% 0%'))
                    .replace(":cool", this.createemojihtml('100% 11%'))   
                    .replace(":facepalm", this.createemojihtml('100% 22%'))   
                    .replace(":gtmp", this.createemojihtml('0% 33%'))
                    .replace(":mute", this.createemojihtml('11% 33%'))   
                    .replace(":devil", this.createemojihtml('22% 33%'))   
                    .replace(":fuck", this.createemojihtml('33% 33%'))   
                    .replace(":beer", this.createemojihtml('44% 33%'))   
                    .replace(":wollow", this.createemojihtml('55% 33%'))

                    .replace(":peace", this.createemojihtml('66% 33%'))
                    .replace(":nerd", this.createemojihtml('77% 33%'))   
                    .replace(":laura", this.createemojihtml('89% 33%'))   
                    .replace(":water", this.createemojihtml('100% 33%'))
                    .replace(":finger", this.createemojihtml('0% 44%'))   
                    .replace(":faust", this.createemojihtml('11% 44%'))   
                    .replace(":rofl", this.createemojihtml('22% 44%'))   
                    .replace(":hugh", this.createemojihtml('33% 44%'))   
                    .replace(":ivan", this.createemojihtml('44% 44%'))

                    .replace(":roxy", this.createemojihtml('55% 44%'))
                    .replace(":audreyshin", this.createemojihtml('66% 44%'))   
                    .replace(":100", this.createemojihtml('77% 44%'))   
                    .replace(":smirking", this.createemojihtml('89% 44%'))
                    .replace(":teddy", this.createemojihtml('100% 44%'))   
                    .replace(":aubergine", this.createemojihtml('0% 55%'))   
                    .replace(":augemachen", this.createemojihtml('11% 55%'))   
                    .replace(":peach", this.createemojihtml('22% 55%'))   
                    .replace(":unschuldig", this.createemojihtml('33% 55%'))

                    .replace(":frog", this.createemojihtml('44% 55%'))
                    .replace(":wave", this.createemojihtml('55% 55%'))   
                    .replace(":gun", this.createemojihtml('66% 55%'))   
                    .replace(":ephi", this.createemojihtml('77% 55%'))
                    .replace(":meth", this.createemojihtml('89% 55%'))   
                    .replace(":dildo", this.createemojihtml('100% 55%'))   
                    .replace(":ballas", this.createemojihtml('0% 66%'))   
                    .replace(":lost", this.createemojihtml('11% 66%'))   
                    .replace(":vagos", this.createemojihtml('22% 66%'))

                    .replace(":lcn", this.createemojihtml('33% 66%'))
                    .replace(":yakuza", this.createemojihtml('44% 66%'))   
                    .replace(":grove", this.createemojihtml('55% 66%'))   
                    .replace(":triaden", this.createemojihtml('66% 66%'))
                    .replace(":mg13", this.createemojihtml('77% 66%'))   
                    .replace(":brigada", this.createemojihtml('89% 66%'))   
                    .replace(":midnight", this.createemojihtml('100% 66%'))   
                    .replace(":weed", this.createemojihtml('0% 77%'))   
                    .replace(":joint", this.createemojihtml('11% 77%'))

                    .replace(":bratwa", this.createemojihtml('22% 77%'))
                    .replace(":lsmc", this.createemojihtml('33% 77%'))   
                    .replace(":diamant", this.createemojihtml('44% 77%'))   
                    .replace(":lspd", this.createemojihtml('55% 77%'))
                    .replace(":dmv", this.createemojihtml('66% 77%'))   
                    .replace(":dpos", this.createemojihtml('77% 77%'))   
                    .replace(":army", this.createemojihtml('89% 77%'))   
                    .replace(":dollar", this.createemojihtml('100% 77%'))   
                    .replace(":fib", this.createemojihtml('0% 88%'))

                    .replace(":weazel", this.createemojihtml('11% 88%'))
                    .replace(":gov", this.createemojihtml('22% 88%'))
                    .replace(":coastguard", this.createemojihtml('33% 88%'))
                    .replace(":airforce", this.createemojihtml('44% 88%'))
                    .replace(":farid", this.createemojihtml('55% 88%'))
                return contentWithEmoji
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
    .senderName {
        float:left;
        font-size: 0.8rem;
    }
    .senderDate {
        float:right;
        font-size: 0.65rem;
    }
    .receiverClass {
        background-color: #f1f8e9;
    }
    .chatMessage {
        padding: 0.1rem;
        border: 0.015rem solid rgba(0,0,0,0.1);
        margin-top: 0.3rem;
    }
    .mapimg{
        width: 20.25vh;
        height: 7.5vh;
        background: url('../../../assets/messenger/map.png') no-repeat;
    }
    .page__content {
        bottom: 4vh !important;
    }
    input.messengerInput {
        margin-top: 0.4rem;
        margin-left: -0.9vh !important;
        width:13vh !important;
        color:black;
        padding: 0.52rem;
        background-color: rgba(255,255,255,0.35);
        border-radius: 0.3rem;
        border-bottom: none;
    }
    .naviCenter{
        margin-left: 9.375vh;
        margin-top: 1.5vh;
    }
    .emoji{
        display: inline-block;
        background-color: #125485; 
        width: 2.5vh; 
        height: 2.5vh; 
        background: url('../../../assets/smartphone/allemoji.png') no-repeat; 
        background-size: 1000%;
    }

    .selectemoji{
        color: #696969;
    }

    .selectemoji:hover{
        color: #1e88e5;
    }
</style>