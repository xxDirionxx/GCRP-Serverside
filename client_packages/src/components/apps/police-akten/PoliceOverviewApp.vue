<template>
    <v-ons-page>
    <v-ons-toolbar>
        <div class="left">
        <v-ons-toolbar-button>
            <v-ons-back-button></v-ons-back-button>
        </v-ons-toolbar-button>
        </div>
        <div class="center">Übersicht {{playerName}}</div>
        <div class="right">
        <v-ons-button modifier="toolbar" style="margin-right: 1rem;" class="color-background blue" v-if="tabs[activeIndex].save" v-on:click="tabs[activeIndex].save">Speichern</v-ons-button>
        </div>
    </v-ons-toolbar>
    <v-ons-tabbar swipeable position="auto" :tabs="tabs" :visible="true" :index.sync="activeIndex"> </v-ons-tabbar>
    </v-ons-page>
</template>

<script>
    import Apps from "../../../apps"
    import PoliceEditPersonApp from "./PoliceEditPersonApp"
    import PoliceEditWantedsApp from "./PoliceEditWantedsApp"
    import PoliceAddAktenApp from "./PoliceAddAktenApp"
    import PoliceListAktenApp from "./PoliceListAktenApp"
    import PoliceListProgressApp from "./PoliceListProgressApp"

    //on click name fetch data and save in store (result.personData result.wanteds ...), on save triggerServer() 

    export default Apps.register({
    name: "PoliceOverviewApp",
    data() {
        return {
        playerName: "",
        activeIndex: 0,
        tabs: [
            { label: "Informationen", page: PoliceEditPersonApp},
            { label: "Wantedvergabe", page: PoliceEditWantedsApp },
            { label: "Aktuelle Akte", page: PoliceAddAktenApp},
            { label: "Verlauf", page: PoliceListProgressApp },
            //{ label: "Aktenübersicht", page: PoliceListAktenApp },
        ],
        }
    },
    methods: {
    },
    props: ['pageStack']
    })
</script>

<style>

</style>
