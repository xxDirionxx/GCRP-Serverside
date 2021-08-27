<template>
  <v-ons-navigator :page-stack="pageStack">
    <component v-for="page in pageStack" :is="page" :key="page.id" :page-stack="pageStack"></component>
  </v-ons-navigator>
</template>
 
<script>
    import Apps from "../apps"
    import DesktopApp from "./DesktopApp"
 
    export default Apps.register({
        name: "ComputerMainScreen",
        data() {
            return {
              pageStack: [DesktopApp]
            }
        }
    })
</script>