<template>
    <v-ons-page>
        <v-ons-toolbar>
            <div class="left">
                <v-ons-toolbar-button>
                    <v-ons-back-button></v-ons-back-button>
                </v-ons-toolbar-button>
            </div>
            <div class="center">Business</div>
        </v-ons-toolbar>
        <v-ons-list>
            <v-ons-list-header>Message Of The Day</v-ons-list-header>
            <v-ons-list-item>{{ motd }}</v-ons-list-item>
        </v-ons-list>
        <v-ons-list>
            <v-ons-list-header>Mitglieder online: {{ anzMembers }}</v-ons-list-header>
            <v-ons-list-item modifier="longdivider" tappable v-for="member in members.BusinessMemberList" :key="member.name">
                <div class="center">
                    <span v-if="member.owner" class="list-item__title" style="font-weight: 600">Inhaber:</span>
                    <span v-if="!member.owner && member.manage" class="list-item__title" style="font-weight: 600">Manager:</span>
                    <span class="list-item__title">{{ member.name }}</span>
                    <span class="list-item__subtitle">Handy: {{ member.number }}
                            <button style="padding-top: 1vh; vertical-align: middle; color: #696969; margin-left: 0.5rem;" class="fas fa-phone fa-1x" v-on:click="call(member.number)"><v-ons-icon icon="md-address-book" class="list-item__icon"></v-ons-icon></button>
                            <button style="padding-top: 1vh; vertical-align: middle; color: #696969;" class="far fa-comments fa-1x" v-on:click="sendsms(member.number)"><v-ons-icon icon="md-address-book" class="list-item__icon"></v-ons-icon></button>
                            <button v-if="members.ManagePermission == 1 || members.ManagePermission == 2" style="padding-top: 1vh; vertical-align: middle; color: #696969;" class="fas fa-user-cog fa-1x" v-on:click="push(member)"><v-ons-icon icon="md-address-book" class="list-item__icon"></v-ons-icon></button>
                    </span>
                </div>
            </v-ons-list-item>
            <section v-show="members.ManagePermission == 1 || members.ManagePermission == 2">
                <v-ons-button class="color-background blue" modifier="large" v-on:click="pushInvite">Mitglied hinzufügen</v-ons-button>
            </section>
        </v-ons-list>
    </v-ons-page>
</template>

<script>
    import Apps from "../../apps"
    import BusinessMemberEditApp from "./BusinessMemberEditApp"
    import BusinessInviteApp from "./BusinessInviteApp"
    import MessengerMessageApp from "../messenger/MessengerMessageApp"

    export default Apps.register({
        name: "BusinessListApp",
        data() {
            return {
                members: [],
                anzMembers: 0,
                motd: ""
            }
        },
        methods: {
            responseBusinessMembers(businessMembersString) {
                this.members = JSON.parse(businessMembersString)
                this.anzMembers = this.members.BusinessMemberList.length
            },
            responseBusinessMOTD(businessMOTD) {
                this.motd = businessMOTD
            },
            push(member) {
                if (this.members.ManagePermission == 1 || this.members.ManagePermission == 2) {
                    this.pageStack.push({ extends: BusinessMemberEditApp, data() { return { member: member } } })
                }
            },
            pushInvite() {
                this.pageStack.push({ extends: BusinessInviteApp })
            },
            call(contactdata){
                this.triggerServer("callUserPhone", contactdata)
            },
            sendsms(contactdata){
                this.pageStack.push({ extends: MessengerMessageApp, data() { return { receiver: contactdata } } })
            }
        },
        mounted() {
            this.triggerServer("requestBusinessMembers")
            this.triggerServer("requestBusinessMOTD")
        },
        props: ['pageStack']
    })
</script>


<!--
components.Hud.visible = true
components.Smartphone.app = "PhoneMainScreen"
components.HomeApp.apps = [{"id": "NewsApp","name": "News","icon": "https://image.flaticon.com/icons/svg/149/149018.svg"}]
    // ATM: [{"id":int, "name": string, "bank": bool, "manage": bool, "salary": int, "owner": bool, "number": int}]
    // ShouldBe: [{"BusinessMemberList":[{"id":int, "name": string, "bank": bool, "manage": bool, "salary": int, "owner": bool, "number": int}], "ManagePermissions": int}]
    // ManagePermissions: [{"type": 1, "name": manage},{"type": 2, "name": owner}]
components.BusinessListApp.members = [{"id":51035,"name":"Simon_Bauer","bank":false,"manage":false,"salary":0,"owner":false,"number":52310},{"id":8888,"name":"Ben_Hooker","bank":false,"manage":true,"salary":0,"owner":false,"number":88},{"id":63124,"name":"Erwin_Fuchs","bank":true,"manage":true,"salary":0,"owner":true,"number":60453}]
-->