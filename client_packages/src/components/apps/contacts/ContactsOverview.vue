<template>
    <v-ons-page>
        <v-ons-toolbar>
            <div class="left">
                <v-ons-toolbar-button>
                    <v-ons-back-button></v-ons-back-button>
                </v-ons-toolbar-button>
            </div>
            <div class="center">Kontakte</div>
            <div class="right">
                <v-ons-toolbar-button>
                    <v-ons-icon icon="ion-edit, material:md-mode-edit" style="font-size: 2vh;" size="25px, material: 20px" v-on:click="editcontact(contactName,contactNumber)"></v-ons-icon>
                </v-ons-toolbar-button>
            </div>
        </v-ons-toolbar>
        <v-ons-list style="overflow:hidden;">
            <v-ons-list-header>Kontakt Übersicht</v-ons-list-header>
            <v-ons-list-item modifier="nodivider" style="padding: 0">
                <div class="center" style="text-align: center;">
                    <h1 style="font-size: 2vh; width: 100%;">{{checkname(contactName)}}</h1>
                </div>
            </v-ons-list-item>
            <v-ons-list-item modifier="nodivider">
                <div class="center" style="text-align: center;">
                    <v-ons-fab modifier="mini" style="background-color: #1e88e5;  margin: 0 0.5vh; margin-left: 5.2vh;" position='center center'>
                        <v-ons-icon style="color:#ffffff; font-size: 2vh; line-height: 0vh;" icon="ion-android-call" v-on:click="callUser"></v-ons-icon>
                    </v-ons-fab>
                    <v-ons-fab modifier="mini" style="background-color: #1e88e5; line-height: 3.5vh; margin: 0 0.5vh;" position='center center'>
                        <v-ons-icon style="color:#ffffff; font-size: 2vh; line-height: 0vh;" icon="ion-android-textsms" v-on:click="sendSMS(contactNumber)"></v-ons-icon>
                    </v-ons-fab>
                    <v-ons-fab v-if="contactName.indexOf('000FAV') >= 0" modifier="mini" style="background-color: #1e88e5; line-height: 3.5vh; margin: 0 0.5vh;" position='center center'>
                        <v-ons-icon style="color:#FFC541; font-size: 2.5vh; line-height: 0vh;" icon="ion-ios-star" v-on:click="setfav(contactName)"></v-ons-icon>
                    </v-ons-fab>
                    <v-ons-fab v-else modifier="mini" style="background-color: #1e88e5; line-height: 3.5vh; margin: 0 0.5vh;" position='center center'>
                        <v-ons-icon style="color:white; font-size: 2.5vh; line-height: 0vh;" icon="ion-ios-star" v-on:click="setfav(contactName)"></v-ons-icon>
                    </v-ons-fab>
                </div>
            </v-ons-list-item>
            <v-ons-list-item modifier="nodivider" style="display: block; height: 3.4vh;">
                <div class="center">
                    Telefon: {{contactNumber}}
                </div>
            </v-ons-list-item>
            <v-ons-list-item modifier="nodivider" style="display: block; height: 3.4vh;">
                <div class="center" style="color: #1e88e5" v-on:click="callUser">
                    Anrufen
                </div>
            </v-ons-list-item>
            <v-ons-list-item modifier="nodivider" style="display: block; height: 3.4vh;">
                <div class="center" style="color: #1e88e5" v-on:click="sendSMS(contactNumber,checkname(contactName))">
                    Nachricht senden
                </div>
            </v-ons-list-item>
            <v-ons-list-item modifier="nodivider" style="display: block; height: 3.4vh;">
                <div v-if="contactName.indexOf('000FAV') >= 0" class="center" style="color: #1e88e5" v-on:click="setfav(contactName)">
                    Von Favoriten enfernen
                </div>
                <div v-else class="center" style="color: #1e88e5" v-on:click="setfav(contactName)">
                    Zu Favoriten hinzufügen
                </div>
            </v-ons-list-item>
            <v-ons-list-item modifier="nodivider" style="display: block; height: 3.4vh;">
                <div class="center" style="color: #1e88e5" v-on:click="sendGPS()">
                    Standort teilen
                </div>
            </v-ons-list-item>
            <v-ons-list-item modifier="nodivider" style="display: block;">
                <div class="center" style="color: #1e88e5" v-on:click="sendContact(contactNumber)">
                    Kontakt teilen
                </div>
            </v-ons-list-item>
        </v-ons-list>
    </v-ons-page>
</template>

<script>
import Apps from "../../apps"
import ContactsEdit from "./ContactsEdit"
import MessageOverviewApp from "../messenger/MessageOverviewApp"
import MessengerContactsApp from "../messenger/MessengerContactsApp"
import MessengerApp from "../messenger/MessengerApp"

export default Apps.register({
    name: "ContactsOverview",
    data() {
        return {
            contactName: "",
            contactNumber: null
        }
    },
    methods: {
        editcontact(contactName, contactNumber) {
            var fav = false
            var name = this.contactName
            if(contactName.indexOf('000FAV') >= 0){
                fav = true
                var contact = name.split('00FAV');
                name = contact[1]
            }
            this.pageStack.push({ extends: ContactsEdit, data(){ return { editName: name, editNumber: contactNumber, storeNumber: contactNumber, favorite: fav }}})
        },
        callUser() {
            this.triggerServer("callUserPhone", this.contactNumber)
        },
        sendSMS(contactNumber, contactName){
            this.pageStack.push({ extends: MessageOverviewApp, data() { return { msgPartner: contactName, msgPartnerNumber: contactNumber } } })
        },
        sendContact(number){
            this.pageStack.push({ extends: MessengerContactsApp, data() { return { number: number } } })
        },
        sendGPS(){
            this.trigger("getLocation", JSON.stringify({ x:0, y:0 }))
        },
        setGPSdata(x, y){
            this.triggerServer("sendPhoneMessage", this.contactNumber, "GPS$$" + x + "$$" + y)
            this.pageStack.pop()
            this.pageStack.pop()
            this.pageStack.push(MessengerApp)
        },
        checkname(name){
            if(name.indexOf('000FAV') >= 0){
                var contact = name.split('00FAV');
                return contact[1];
            }else{
                return name;
            }
        },
        setfav(name){
            if(name.indexOf('000FAV') >= 0){
                var contact = name.split('00FAV');
                this.trigger("updateContact", JSON.stringify({ storeNumber: this.contactNumber, editNumber: this.contactNumber, editName: contact[1] }))
                this.contactName = contact[1]
            }else{
                this.contactName = "000FAV" + name;
                this.trigger("updateContact", JSON.stringify({ storeNumber: this.contactNumber, editNumber: this.contactNumber, editName: this.contactName }))
            }
        }
    },
    props: ['pageStack']
})
</script>