<template>
  <v-ons-page>
    <v-ons-toolbar>
      <div class="left">
        <v-ons-toolbar-button>
          <v-ons-back-button></v-ons-back-button>
        </v-ons-toolbar-button>
      </div>
      <div class="center">{{member.name}}</div>
    </v-ons-toolbar>
      <v-ons-list>
        <v-ons-list-header>Rechte Verwaltung</v-ons-list-header>
        <v-ons-list-item modifier="longdivider">
          <div class="center">
            Verwaltungsrechte
          </div>
          <div class="right">
            <v-ons-switch :checked="member.manage" v-model="member.manage"></v-ons-switch>
          </div>
        </v-ons-list-item>
        <v-ons-list-item modifier="longdivider">
          <div class="center">
            Bankrechte
          </div>
          <div class="right">
            <v-ons-switch :checked="member.bank" v-model="member.bank"></v-ons-switch>
          </div>
        </v-ons-list-item>
        <v-ons-list-item modifier="longdivider">
          <div class="center">
            Inventarrechte
          </div>
          <div class="right">
            <v-ons-switch :checked="member.inventory" v-model="member.inventory"></v-ons-switch>
          </div>
        </v-ons-list-item>
        <v-ons-list-header>Rang: {{ member.rank }}</v-ons-list-header>
        <v-ons-list-item modifier="longdivider">
          <v-ons-button style="margin-right: 9px;" modifier="large" v-on:click="rankUp">Rank up</v-ons-button>
        </v-ons-list-item>
        <v-ons-list-item modifier="longdivider">
          <v-ons-button style="margin-right: 9px;" modifier="large" v-on:click="rankDown">Rank down</v-ons-button>
        </v-ons-list-item>
        <v-ons-list-item>
          <v-ons-button style="margin-right: 9px;" modifier="large" v-on:click="handle">Speichern</v-ons-button>
        </v-ons-list-item>
        <v-ons-list-item>
          <v-ons-button class="color-background red" style="margin-right: 9px;" modifier="large" v-on:click="kickMember">Entlassen</v-ons-button>
        </v-ons-list-item>
      </v-ons-list>
  </v-ons-page>
</template>

<script>
  import Apps from "../../apps"

  export default Apps.register({
    name: "TeamMemberEditApp",
    data() {
      return {
        member: null,
        manage: false,
        rank: 0
      }
    },
    methods: {
      rankUp(){
        if(this.member.rank == 12) return
        this.member.rank++
      },
      rankDown(){
        if(this.member.rank == 0) return
        this.member.rank--
      },
      handle() {
        if (this.member.id == null || this.member.rank == null) return
        this.triggerServer("editTeamMember", this.member.id, this.member.rank, this.member.bank, this.member.inventory, this.member.manage)
      },
      kickMember() {
        if(this.member.id == null) return
        this.triggerServer("kickMember", this.member.id)
      }
    },
    created() {
      if(this.member.manage == 2) {
        this.manage = true
      }
      else {
        this.manage = false
      }
    },
    props: ['pageStack']
  })
</script>