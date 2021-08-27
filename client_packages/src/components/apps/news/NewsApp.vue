<template>
    <v-ons-page>
        <component v-for="tab in tabs" :is="tab.page" :key="tab.label" :page-stack="pageStack"></component>
    </v-ons-page>
</template>

<script>
    import Apps from "../../apps"
    import NewsListApp from "./NewsListApp"

    export default Apps.register({
        name: "NewsApp",
        data() {
            return {
                activeIndex: 0,
                tabs: [
                    {
                        label: "Berichte",
                        page: NewsListApp
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