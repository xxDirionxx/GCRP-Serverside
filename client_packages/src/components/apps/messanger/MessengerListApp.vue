<template>
    <v-ons-page>
        <v-ons-toolbar>
            <div class="left">
                <v-ons-toolbar-button>
                    <v-ons-icon icon="md-arrow-back, material:md-arrow-back" size="material: 1.875vh" v-on:click="backBtn"></v-ons-icon>
                </v-ons-toolbar-button>
            </div>
            <div class="center" style="margin-left: -20px">Nachrichten</div>
        </v-ons-toolbar>
        <v-ons-list>
            <v-ons-list-item v-if="messages.length == 0">
                <div class="center">Keine Nachrichten vorhanden</div>
            </v-ons-list-item>
            <v-ons-list-item modifier="longdivider" tappable v-for="chats in messages" :key="chats.id" v-on:click="push(chats)">
                <div class="left">
                    <v-ons-icon icon="md-comments" class="list-item__icon"></v-ons-icon>
                </div>
                <div class="center">
                    {{checkname(chats.messageSender)}}
                </div>
                <div class="right smallFont">
                    {{chats.lastMessage}}
                </div>
            </v-ons-list-item>
        </v-ons-list>
        <v-ons-fab modifier="mini" position='bottom right' style="background-color: #1e88e5;">
            <v-ons-icon icon="md-plus" v-on:click="addMessage" style="color: #ffffff;"></v-ons-icon>
        </v-ons-fab>
    </v-ons-page>
</template>

<script>
    import Apps from "../../apps"
    import MessageOverviewApp from "./MessageOverviewApp"
    import MessengerAddContactsApp from "./MessengerAddContactsApp"

    export default Apps.register({
        name: "MessengerListApp",
        data() {
            return {
                messages: [],
                mode: false,
                msgPartnerNumber: null,
                chats: []
            }
        },
        methods: {
            responseKonversations(response) {
                this.messages = JSON.parse(response)
                if(this.mode == true){
                    for(this.chats in this.messages){
                        if(this.msgPartnerNumber == this.chats.messageSenderNumber){
                            this.push(this.chats);
                            this.mode = false;
                            this.msgPartnerNumber = null;
                        }
                    }
                }
            },
            push(chats) {
                this.pageStack.push({ extends: MessageOverviewApp, data() { return { messages: chats.messages, msgPartner: chats.messageSender, msgPartnerNumber: chats.messageSenderNumber, msgId: chats.messagesId } } })
            },
            addMessage(){
                this.pageStack.push({ extends: MessengerAddContactsApp})
            },
            checkname(name){
                if(name.indexOf('000FAV') >= 0){
                    var contact = name.split('00FAV');
                    return contact[1];
                }else{
                    return name;
                }
            },
            backBtn() {
                this.trigger("getHomeScreen",[])
            }
        },
        mounted() {
            this.triggerServer("requestKonversations")
        },
        computed: {
            changeCount: function () {
                return this.newMessageCount = 5
            }
        },
        props: ['pageStack']
    })
</script>
<style lang="scss" scoped>
    .list-item--material__left {
        min-width: 2rem;
    }
    .smallFont {
        font-size: 0.7rem;
    }
</style>
<!-- [{"messagesId":1,"messageSender":"Willhelm Hartmut","lastMessage":"19:50","messages":[{"id":1,"sender":"adasd","date":"17:45","message":"Hallo","receiver":false},{"id":2,"sender":"DEF","date":"17:46","message":"Hallo","receiver":true}]},{"messagesId":2,"messageSender":"Franz Eckbert","lastMessage":"Gestern","messages":[{"id":563,"sender":"adasd","date":"Gestern","message":"Hallo","receiver":false},{"id":1516,"sender":"DEF","date":"Gestern","message":"Hallo","receiver":true}]}] -->