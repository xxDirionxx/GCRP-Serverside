<template>
    <v-ons-page>
        <v-ons-toolbar>
            <div class="left">
                <v-ons-toolbar-button>
                    <v-ons-back-button class="pointerClass"></v-ons-back-button>
                </v-ons-toolbar-button>
            </div>
            <div class="center">Service</div>
        </v-ons-toolbar>
        <v-ons-list>
            <v-ons-list-header>Anfrage senden - {{ category.name }}</v-ons-list-header>
            <v-ons-list-item modifier="longdivider">
                <div class="center">
                    <input style="width:100%; margin-right: 10px;" placeholder="Beschreibung eingeben" maxlength="100" v-model="content" />
                </div>
            </v-ons-list-item>
            <v-ons-list-item modifier="longdivider">
              <div class="center">
                  Telefonnummer senden: 
              </div>
              <div class="right">
                  <v-ons-switch :checked="phone" v-model="phone"></v-ons-switch>
              </div>
          </v-ons-list-item>
            <v-ons-list-item modifier="longdivider">
                <v-ons-button style="width:100%; margin-right: 10px; margin-top: -1px;" class="color-background blue" modifier="large" @click="sendRequest">Anfrage senden</v-ons-button>
            </v-ons-list-item>
        </v-ons-list>
    </v-ons-page>
</template>

<script>
    import Apps from "../../apps"

    export default Apps.register({
        name: "ServiceSendRequestApp",
        data() {
            return {
                category: [],
                content: '',
                phone: false
            }
        },
        methods: {
            sendRequest() {
                if (this.category.id == null && this.content.trim() == "") return
                this.triggerServer("addServiceRequest", this.category.service, this.content, this.phone)
            }
        },
        props: ['pageStack']
    })
</script>

<style lang="scss" scoped>
    section {
        margin: 20px;
    }
    .fRow {
        padding: 1rem 0 0 1rem;
    }
</style>