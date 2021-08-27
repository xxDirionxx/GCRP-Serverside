<template>
    <v-ons-page>
        <v-ons-toolbar>
            <div class="left">
                <v-ons-toolbar-button>
                    <v-ons-back-button class="pointerClass"></v-ons-back-button>
                </v-ons-toolbar-button>
            </div>
            <div class="center">Service</div>
        </v-ons-toolbar>
        <v-ons-list>
            <v-ons-list-header>Kategorien</v-ons-list-header>
            <v-ons-list-item modifier="longdivider" tappable v-for="category in categories" :key="category.id" @click="pushCategory(category)">
                <div class="center">{{ category.name }}</div>
                <div class="right">
                    <v-ons-icon icon="md-chevron-right" class="list-item__icon"></v-ons-icon>
                </div>
            </v-ons-list-item>
            <v-ons-list-item modifier="longdivider">
                <v-ons-button style="width:100%; margin-right: 10px; margin-top: -1px;" class="color-background red" modifier="large" @click="cancelRequests">Anfrage abbrechen</v-ons-button>
            </v-ons-list-item>
        </v-ons-list>
    </v-ons-page>
</template>

<script>
    import Apps from "../../apps"
    import ServiceSendRequestApp from "./ServiceSendRequestApp"

    export default Apps.register({
        name: "ServiceRequestApp",
        data() {
            return {
                categories: [
                    {
                        "id": 1,
                        "name": "Polizei",
                        "service": "police"
                    },
                    {
                        "id": 2,
                        "name": "Rettungsdienst",
                        "service": "medic"
                    },
                    {
                        "id": 3,
                        "name": "Fahrschule",
                        "service": "fahrlehrer"
                    },
                    {
                        "id": 4,
                        "name": "DPOS",
                        "service": "tow"
                    },
                    {
                        "id": 5,
                        "name": "News",
                        "service": "news"
                    },
                    {
                        "id": 6,
                        "name": "Los Santos Customs",
                        "service": "lsc"
                    },
                    {
                        "id": 7,
                        "name": "Regierung",
                        "service": "government"
                    },
                    {
                        "id": 8,
                        "name": "US Army",
                        "service": "army"
                    },
                    {
                        "id": 9,
                        "name": "Auktion",
                        "service": "auction"
                    }
                ]
            }
        },
        methods: {
          pushCategory(selectedCategory) {
            this.pageStack.push({ extends: ServiceSendRequestApp, data(){ return { category: selectedCategory }}})
          },
          cancelRequests(){
              this.triggerServer("cancelServiceRequest")
          }
        },
        props: ['pageStack']
    })
</script>

<style lang="scss" scoped>

</style>