<template>
  <v-ons-page>
    <v-ons-toolbar>
      <div class="left">
        <v-ons-toolbar-button>
          <v-ons-back-button></v-ons-back-button>
        </v-ons-toolbar-button>
      </div>
      <div class="center">Profil</div>
    </v-ons-toolbar>
    <v-ons-list>
        <v-ons-list-item v-for="(value, key) in profileData" :key="value.playerId" modifier="longdivider"><b>{{ key }}</b>: {{ value }}</v-ons-list-item>
        <ons-list-header>Eventpunkte</ons-list-header>
        <v-ons-list-item v-for="(value, key) in specialData" :key="key" modifier="longdivider"><b>{{ key }}</b>: {{ value }}</v-ons-list-item>
    </v-ons-list>
  </v-ons-page>
</template>

<script>
import Apps from "../apps"

export default Apps.register({
    name: "ProfileApp",
    data() {
        return {
            profileData: [],
            specialData:[]
        }
    },
    methods: {
        setProfileData(data) {
            this.profileData = JSON.parse(data)
        },
        responseSpecialProfilData(data) {
            this.specialData = JSON.parse(data)
        }
    },
    mounted() {
        this.triggerServer('requestSpecialProfilData')
    },
    props: ['pageStack']
})
</script>