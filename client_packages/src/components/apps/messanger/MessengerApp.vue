<template>
    <v-ons-page>
        <component v-for="tab in tabs" :is="tab.page" :key="tab.page.label" :page-stack="pageStack"></component>
    </v-ons-page>
</template>

<script>
    import Apps from "../../apps"
    import MessengerListApp from "./MessengerListApp"
    import MessengerMessageApp from "./MessengerMessageApp"

    export default Apps.register({
        name: "MessengerApp",
        data() {
            return {
                activeIndex: 0,
                tabs: [
                    {
                        label: "Übersicht",
                        page: MessengerListApp
                    }
                ]
            }
        },
        props: ['pageStack']
    })
</script>
<style lang="scss" scoped>
    .list-item--material {
        padding: 5px 0 5px 16px;
    }

    .card {
        padding: 0;
    }
</style>