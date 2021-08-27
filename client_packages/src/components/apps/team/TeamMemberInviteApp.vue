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
      <v-ons-list-header>Mitglied einladen</v-ons-list-header>
      <v-ons-list-item modifier="longdivider">
        <div class="center">
          <input style="width:100%; margin-right: 10px;" placeholder="Name eingeben" v-model="playerName" />
        </div>
      </v-ons-list-item>
      <v-ons-list-item modifier="longdivider">
        <v-ons-button style="width:100%; margin-right: 10px; margin-top: -1px;" class="color-background blue" modifier="large" v-on:click="invitePlayer">Einladen</v-ons-button>
      </v-ons-list-item>
    </v-ons-list>
  </v-ons-page>
</template>

<script>
    import Apps from "../../apps"

    export default Apps.register({
        name: "TeamMemberInviteApp",
        data() {
            return {
              playerName: ""
            }
        },
        methods: {
            invitePlayer(){
              if(this.playerName == "") return
              this.triggerServer("addPlayer", this.playerName)
            }
        },
        props: ['pageStack']
    })
</script>