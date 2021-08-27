<template>
    <v-ons-page>
        <v-ons-toolbar>
            <div class="left">
                <v-ons-toolbar-button>
                    <v-ons-back-button></v-ons-back-button>
                </v-ons-toolbar-button>
            </div>
            <div class="center">Kontakte</div>
            <div class="right">
                <v-ons-toolbar-button>
                    <v-ons-icon icon="ion-trash-a, material:md-delete" size="25px, material: 20px" v-on:click="deleteContact"></v-ons-icon>
                </v-ons-toolbar-button>
            </div>
        </v-ons-toolbar>
        <v-ons-list>
            <v-ons-list-header>Kontakt editieren</v-ons-list-header>
            <v-ons-list-item modifier="nodivider">
                <div class="center">
                    <input style="width:100%; margin-right: 0.9375vh;" placeholder="Name" type="text" v-model="editName" />
                </div>
            </v-ons-list-item>
            <v-ons-list-item modifier="nodivider">
                <div class="center">
                    <input style="width:100%; margin-right: 0.9375vh;" placeholder="Telefonnummer" type="number" v-model="editNumber" />
                </div>
            </v-ons-list-item>
            <v-ons-list-item modifier="nodivider">
                <div class="center">
                    <v-ons-button style="width:100%; margin-right: 0.9375vh;" class="color-background blue" modifier="large" v-on:click="updateContact">Speichern</v-ons-button>
                </div>
            </v-ons-list-item>
        </v-ons-list>
    </v-ons-page>
</template>

<script>
import Apps from "../../apps"

export default Apps.register({
    name: "ContactsEdit",
    data() {
        return {
            favorite: false,
            editName: "",
            editNumber: null,
            storeNumber: null,
            newMessage: ""
        }
    },
    methods: {
        updateContact() {
          if(this.storeNumber == null || this.editNumber == null || this.editName == "") return
            for (this.charAmount = 0; this.charAmount < this.editName.length; this.charAmount++) { 
                if(['"',"'",'{','}',']','[',';','\\'].indexOf(this.editName[this.charAmount]) >= 0){
                this.newMessage += ' '
                } else {
                this.newMessage += this.editName[this.charAmount]
                }                  
            }
            this.editName = ""
            this.editName = this.newMessage
            if(this.favorite == true){
                this.editName = "000FAV" + this.editName
            }
            this.newMessage = ""
            this.trigger("updateContact", JSON.stringify({ storeNumber: this.storeNumber, editNumber: this.editNumber, editName: this.editName }))
            this.pageStack.pop()
        },
        deleteContact() {
            if (this.storeNumber == null) return
            this.trigger("removeContact", JSON.stringify({ number: this.storeNumber }))
            this.pageStack.pop()
        }
    },
    props: ['pageStack']
})
</script>