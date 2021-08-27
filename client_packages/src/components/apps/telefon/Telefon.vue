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
import PhoneInput from "./TelefonInput"
import PhoneSettings from "./TelefonSettings"
import PhoneHistory from "./TelefonCalls"

export default Apps.register({
    name: "Telefon",
    data() {
        return {
            activeIndex: 0,
            tabs: [
                {
                    label: "Telefon",
                    page: PhoneInput
                },
                {
                    label: "Verlauf",
                    page: PhoneHistory
                }
            ]
        }
    },
    props: ['pageStack']
})
</script>