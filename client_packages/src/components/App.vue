<template>
  <div id="app">
    <!--<img src="./assets/logo.png">-->
    <Windows/>
    <Hud/>
  </div>
</template>

<script>
import Component from "vue-class-component"
import Vue from "vue"
import Windows from "./components/Windows.vue"
import Hud from "./components/hud/Hud.vue"

@Component({
    components: {
        Windows,
        Hud
    }
})
export default class App extends Vue {}
</script>

<style lang="scss">
@import "../stylesheets/ingame.scss";
@import "../stylesheets/fuchsi.css";
/* Test */
@import "../stylesheets/components/devices/devices.scss";
/*
@import "../stylesheets/components/devices/normalize.scss";
@import "../stylesheets/components/devices/style.scss";
    */

/*#app {
  font-family: "Avenir", Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin-top: 60px;
}*/
body {
    touch-action: initial !important;
}
</style>
