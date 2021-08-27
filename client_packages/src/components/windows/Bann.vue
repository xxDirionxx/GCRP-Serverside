<template>
  <div>
    <img class="logo" src="../../assets/logo_christmas.png">
    <div class="jumbotron center card">
      <h4>
        Willkommen zurück {{ player.name }}
      </h4>
      <p class="color-text red">
        Du wurdest von einem Administrator gebannt, <br> bitte melde dich auf unserem Teamspeak Server: <b>ts....</b> im Support.
      </p>
    </div>
  </div>
</template>

<script>
import Windows from "../windows"

export default Windows.register({
  name: "Bann",
  props: {
    data: String
  },
  data() {
    return {
      player: JSON.parse(this.data)
    }
  }
})
</script>

<!-- components.get("Windows").show("Bann", '{"name": "bla"}') -->