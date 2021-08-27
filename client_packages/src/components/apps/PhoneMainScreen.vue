<template>
  <v-ons-navigator :page-stack="pageStack">
    <component v-for="page in pageStack" :is="page" :key="page.id" :page-stack="pageStack"></component>
  </v-ons-navigator>
</template>

<script>
    import Apps from "../apps"
    import HomeApp from "./HomeApp"
 
    export default Apps.register({
        name: "PhoneMainScreen",
        data() {
            return {
              pageStack: [HomeApp]
            }
        },
        methods: {
            getHomeScreen(){
                this.pageStack = [HomeApp]
            }
        }
    })
</script>