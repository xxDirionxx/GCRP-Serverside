<template>
    <v-ons-page>
        <v-ons-toolbar>
            <div class="left">
                <v-ons-toolbar-button>
                    <v-ons-back-button></v-ons-back-button>
                </v-ons-toolbar-button>
            </div>
            <div class="center">Meine Anfragen</div>
        </v-ons-toolbar>

        <v-ons-list v-if="taxiDuty">
            <v-ons-list-header>Offene Auftraege ({{services.length}})</v-ons-list-header>
            <v-ons-list-item v-if="services.length == 0">
                <div class="center">Keine offenen Auftraege</div>
            </v-ons-list-item>
            <v-ons-list-item v-else modifier="longdivider" tappable v-for="service in services" :key="service.id" v-on:click="push(service.name, service.message, service.id)">
                <div class="center">
                    <span class="list-item__title">{{service.name}}</span>
                </div>
                <div class="right">
                    <v-ons-icon icon="md-chevron-right" class="list-item__icon"></v-ons-icon>
                </div>
            </v-ons-list-item>
        </v-ons-list>
        <v-ons-list v-else>
            <v-ons-list-item>
                <div class="center">Sie sind nicht im Taxi Dienst!</div>
            </v-ons-list-item>
        </v-ons-list>
    </v-ons-page>
</template>

<script>
    import Apps from "../../apps"
    import TaxiServiceContactApp from "./TaxiServiceContactApp"

    export default Apps.register({
        name: "TaxiServiceListApp",
        data() {
            return {
                services: [],
                taxiDuty: false
            }
        },
        methods: {
            push(serviceName, serviceMessage, serviceSpielerId) {
                this.pageStack.push({ extends: TaxiServiceContactApp, data() { return { name: serviceName, message: serviceMessage, spielerId: serviceSpielerId } } })
            },
            responseServiceList(taxiDuty,serviceList) {
                this.services = JSON.parse(serviceList)
                this.taxiDuty=taxiDuty
            }
        },
        mounted() {
            this.triggerServer("requestTaxiServiceList")
        },
        props: ['pageStack']
    })
</script>