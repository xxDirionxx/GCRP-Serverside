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
    import BankAppOverview from "./BankAppOverview"
    import BankAppTransfer from "./BankAppTransfer"
    export default Apps.register({
        name: "BankApp",
        data() {
            return {
                activeIndex: 0,
                tabs: [
                    {
                        label: "Übersicht",
                        page: BankAppOverview
                    },
                    {
                        label: "Überweisung",
                        page: BankAppTransfer
                    }
                ]
            }
        },
        watch: {
        },

        props: ['pageStack']
    })
</script>