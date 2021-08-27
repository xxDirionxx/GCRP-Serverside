<template>
  <v-ons-page>
    <v-ons-toolbar>
      <div class="left">
        <v-ons-toolbar-button>
          <v-ons-back-button></v-ons-back-button>
        </v-ons-toolbar-button>
      </div>
      <div class="center">Kontakte</div>
    </v-ons-toolbar>
    <v-ons-list>
      <v-ons-list-header>Kontakt hinzufügen</v-ons-list-header>
      <v-ons-list-item modifier="longdivider">
        <div class="center">
          <input style="width:100%; margin-right: 0.9375vh;" placeholder="Name" type="text" v-model="name" />
        </div>
      </v-ons-list-item>
      <v-ons-list-item modifier="longdivider">
        <div class="center">
          <input style="width:100%; margin-right: 0.9375vh;" placeholder="Nummer" v-model="number" type="number" />
        </div>
      </v-ons-list-item>
      <v-ons-list-item modifier="longdivider">
        <v-ons-button style="width:100%; margin-right: 0.9375vh; margin-top: -1px;" class="color-background blue" modifier="large" v-on:click="addContact()">Kontakt hinzufügen</v-ons-button>
      </v-ons-list-item>
    </v-ons-list>
  </v-ons-page>
</template>

<script>
  import Apps from "../../apps"

  export default Apps.register({
    name: "ContactsAdd",
    data() {
      return {
        name: "",
        number: null,
        newMessage: ""
      }
    },
    methods: {
      addContact(){
        if(this.name == "" || this.number == null) return
          for (this.charAmount = 0; this.charAmount < this.name.length; this.charAmount++) { 
            if([' ','ä','ö','ü','ß','a',"b",'c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z','0','1','2','3','4','5','6','7','8','9',',','.','-','_','|','§','$','%','&','#',':','*','+','?','!','='].indexOf(this.name[this.charAmount].toLowerCase()) == -1){
              this.newMessage += '�'
            } else {
              this.newMessage += this.name[this.charAmount]
            }                  
          }
          this.name = ""
          this.name = this.newMessage
          this.newMessage = ""
          this.trigger("addContact", JSON.stringify({ name: this.name, number: this.number }))
          this.pageStack.pop()
      }
    },
    props: ['pageStack']
  })
</script>