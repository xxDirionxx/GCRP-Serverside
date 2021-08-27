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
      <v-ons-list-header>Mitgliedsverwaltung</v-ons-list-header>
      <v-ons-list>
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
                  Raffenerie
              </div>
              <div class="right">
                  <v-ons-switch :checked="member.raffinery" v-model="member.raffinery"></v-ons-switch>
              </div>
          </v-ons-list-item>
          <v-ons-list-item modifier="longdivider">
              <div class="center">
                  Tankstelle
              </div>
              <div class="right">
                  <v-ons-switch :checked="member.fuelstation" v-model="member.fuelstation"></v-ons-switch>
              </div>
          </v-ons-list-item>
          <v-ons-list-item modifier="longdivider">
              <div class="center">
                  Nightclub
              </div>
              <div class="right">
                  <v-ons-switch :checked="member.nightclub" v-model="member.nightclub"></v-ons-switch>
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
                  Tätowierer
              </div>
              <div class="right">
                  <v-ons-switch :checked="member.tattoo" v-model="member.tattoo"></v-ons-switch>
              </div>
          </v-ons-list-item>
          <v-ons-list-item modifier="longdivider">
              <div class="center">
                  <input style="width:100%; margin-right: 10px;" placeholder="Gehalt eingeben" int v-model="member.salary" />
              </div>
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
    name: "BusinessMemberEditApp",
    data() {
      return {
        member: null
      }
    },
    methods: {
      handle() {
        if (this.member.id == null) return
          this.triggerServer("editBusinessMember", this.member.id, this.member.bank, this.member.manage, this.member.salary, this.member.raffinery, this.member.fuelstation, this.member.nightclub, this.member.tattoo)
          this.pageStack.pop()
      },
      kickMember() {
        if(this.member.id == null) return
          this.triggerServer("kickBusinessMember", this.member.id)
          this.pageStack.pop()
      }
    },
    props: ['pageStack']
  })
</script>