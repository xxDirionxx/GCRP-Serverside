<template>
    <v-ons-page>
        <v-ons-tabbar position="bottom" :tabs="tabs" :visible="true" :index.sync="activeIndex">
            <template slot="pages">
                <component v-for="tab in tabs" :is="tab.page" :key="tab.page.label" :page-stack="pageStack"></component>
            </template>
        </v-ons-tabbar>
    </v-ons-page>
</template>

<script>
    import Apps from "../../apps"
    import TaxiListApp from "./TaxiListApp"
    import TaxiServiceListApp from "./TaxiServiceListApp"

    export default Apps.register({
        name: "TaxiApp",
        data() {
            return {
                activeIndex: 0,
                tabs: [
                    {
                        label: "Taxi",
                        page: TaxiListApp
                    },
                    {
                        label: "Anfragen",
                        page: TaxiServiceListApp
                    }
                ]
            }
        },
        props: ['pageStack']
    })
</script>