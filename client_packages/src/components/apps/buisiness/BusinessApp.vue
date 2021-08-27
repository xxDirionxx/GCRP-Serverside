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
import BusinessListApp from "./BusinessListApp"
import BusinessEditApp from "./BusinessEditApp"

export default Apps.register({
    name: "BusinessApp",
    data() {
        return {
            activeIndex: 0,
            tabs: [
                {
                    label: "Members",
                    page: BusinessListApp
                },
                {
                    label: "Settings",
                    page: BusinessEditApp
                }
            ]
        }
    },
    props: ['pageStack']
})
</script>