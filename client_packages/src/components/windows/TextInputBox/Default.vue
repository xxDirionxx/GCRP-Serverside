<template>
  <div>
    <div class="jumbotron center card">
      <h4 id="title">{{ inputData.textBoxObject.Title }}</h4>
      <h6>
        {{ inputData.textBoxObject.Message }}
      </h6>
      <div class="form">
        <div class="input-group">
          <input placeholder="Text eingeben" type="text" v-model="content" autofocus/>
        </div>
        <button class="btn secondary" v-on:click="handle">Absenden</button>
        <button class="btn flat neutral" v-on:click="cancel">Abbrechen</button>
      </div>
    </div>
  </div>
</template>

<script>
import Windows from "../../windows"

export default Windows.register({
    name: "Default",
    props: {
        data: String
    },
    data() {
        return {
          content: "",
          inputData: JSON.parse(this.data),
        }
    },
    methods: {
     handle() {
        if(this.content == "") return
        this.triggerServer(this.inputData.textBoxObject.Callback, this.content)
        this.dismiss()
     },cancel(){
       this.dismiss()
     }
    }
})
</script>

<style lang="scss" scoped>

</style>