<template>
  <v-ons-navigator :page-stack="pageStack">
    <component v-for="page in pageStack" :is="page" :key="page.id" :page-stack="pageStack"></component>
  </v-ons-navigator>
</template>
 
<script>
    import Apps from "../apps"
    import IpadDesktopApp from "./IpadDesktopApp"
 
    export default Apps.register({
        name: "IpadMainScreen",
        data() {
            return {
              pageStack: [IpadDesktopApp]
            }
        }
    })
</script>