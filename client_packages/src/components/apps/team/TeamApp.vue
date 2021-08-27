<template>
  <v-ons-page>
    <v-ons-toolbar>
      <div class="left">
        <v-ons-toolbar-button>
          <v-ons-back-button></v-ons-back-button>
        </v-ons-toolbar-button>
      </div>
      <div class="center">Team</div>
    </v-ons-toolbar>
    <v-ons-list>
      <v-ons-list-header>Mitglieder online ({{ anzMembers }})</v-ons-list-header>

      <v-ons-list-item modifier="longdivider" tappable v-for="member in members.TeamMemberList" :key="member.name">
        <div class="center">
          <span class="list-item__title">
            Rang: {{member.rank}}
          </span>
          <span class="list-item__subtitle">{{member.name}}</span>
          <span class="list-item__subtitle">Telefon: {{member.number}}
            <button style="padding-top: 1vh; vertical-align: middle; color: #696969; margin-left: 0.5rem;" class="fas fa-phone fa-1x" v-on:click="call(member.number)"><v-ons-icon icon="md-address-book" class="list-item__icon"></v-ons-icon></button>
            <button style="padding-top: 1vh; vertical-align: middle; color: #696969;" class="far fa-comments fa-1x" v-on:click="sendsms(member.number)"><v-ons-icon icon="md-address-book" class="list-item__icon"></v-ons-icon></button>
            <button v-if="members.ManagePermission == 1 || members.ManagePermission == 2" style="padding-top: 1vh; vertical-align: middle; color: #696969;" class="fas fa-user-cog fa-1x" v-on:click="push(member)"><v-ons-icon icon="md-address-book" class="list-item__icon"></v-ons-icon></button>
          </span>
        </div>
        <div class="right">
            <span v-if="member.manage == 1 || member.manage == 2">
                <b>L</b>
            </span>
        </div>
      </v-ons-list-item>
    </v-ons-list>

    <section v-show="members.ManagePermission == 1 || members.ManagePermission == 2">
      <v-ons-button class="color-background blue" modifier="large" v-on:click="pushInvite">Einladen</v-ons-button>
    </section>

  </v-ons-page>
</template>

<script>
  import Apps from "../../apps"
  import TeamMemberEditApp from "./TeamMemberEditApp"
  import TeamMemberInviteApp from "./TeamMemberInviteApp"
  import MessengerMessageApp from "../messenger/MessengerMessageApp"

  export default Apps.register({
    name: "TeamListApp",
    data() {
      return {
          members: [],
          anzMembers: 0
      }
    },
    methods: {
      responseTeamMembers(teamMembersString) {
          this.members = JSON.parse(teamMembersString)
          this.anzMembers = this.members.TeamMemberList.length
      },
      push(member) {
          if(this.members.ManagePermission == 1 || this.members.ManagePermission == 2){
              this.pageStack.push({ extends: TeamMemberEditApp, data(){ return { member: member }}})
          }
      },
      pushInvite(){
        if(this.members.ManagePermission == 1 || this.members.ManagePermission == 2){
          this.pageStack.push({ extends: TeamMemberInviteApp })
        }
      },
      call(contactdata){
          this.triggerServer("callUserPhone", contactdata)
      },
      sendsms(contactdata){
          this.pageStack.push({ extends: MessengerMessageApp, data() { return { receiver: contactdata } } })
      }
    },
    props: ['pageStack']
  })
</script>