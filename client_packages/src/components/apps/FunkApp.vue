<template>
    <v-ons-page>
        <v-ons-toolbar>
            <div class="left">
                <v-ons-toolbar-button>
                    <v-ons-back-button></v-ons-back-button>
                </v-ons-toolbar-button>
            </div>
            <div class="center">Funkgerät</div>
        </v-ons-toolbar>

        <v-ons-list>
            <v-ons-list-header>Aktueller Funk: <span style="font-weight:bold">{{voiceRoom}}</span></v-ons-list-header>
            <v-ons-list-item modifier="nodivider">
                <input style="width:100%; margin-right: 10px;" v-model="voiceRoom" placeholder="Funkkanal" />
            </v-ons-list-item>

            <v-ons-list-item modifier="nodivider" style="margin-top: -15px;">
                <v-ons-button style="margin-right: 10px;" class="color-background blue" modifier="large" v-on:click="joinVoice">Betreten</v-ons-button>
            </v-ons-list-item>
            <v-ons-list-item v-show="errormsg!=''" style="color:red">{{errormsg}}</v-ons-list-item>
            <v-ons-list-item modifier="longdivider" v-for="state in states" :key="state.value" v-on:click="handle(state.value)" tappable>
                <label class="left">
                    <v-ons-radio :input-id="'radio-' + state.value" :value="state.value" name="options" v-model="active"></v-ons-radio>
                </label>
                <label :for="'radio-' + state.value" class="center">
                    {{ state.text }}
                </label>
            </v-ons-list-item>

        </v-ons-list>
    </v-ons-page>
</template>

<script>
    import Apps from "../apps"

    export default Apps.register({
        name: "FunkApp",
        data() {
            return {
                active: 0,
                states: [
                    { text: 'Funk aus', value: 0 },
                    { text: 'Funk PushToTalk', value: 1 },
                    { text: 'Funk Dauersenden', value: 2 }
                ],
                voiceRoom: "",
                abusetime: 20,
                timer: null,
                errormsg:""
            }
        },
        methods: {
            handle(state) {
                this.triggerServer("changeSettings", state)
                this.trigger("setVoiceRadioActive", true)
                this.trigger("setVoiceRadioActiveType", state)
            },
            joinVoice() {
                if (isNaN(this.voiceRoom)) return
                if ((this.timer + 20000) >= Date.now()) {
                    this.errormsg = "Funkfrequenz überlastet, bitte probieren Sie es in 20 Sekunden erneut.";
                    return;
                } else {
                    this.errormsg = "";
                }
                this.timer = Date.now();
                this.triggerServer("changeFrequenz", this.voiceRoom)
            },
            responseVoiceSettings(response){
                let voice = JSON.parse(response)
                this.voiceRoom = voice.Room
                this.active = voice.Active
            }
        },
        mounted() {
            this.triggerServer("requestVoiceSettings")
        },
        props: ['pageStack']
    })
</script>

<style scoped>

</style>