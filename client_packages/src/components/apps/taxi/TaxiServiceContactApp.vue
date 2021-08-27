<template>
	<v-ons-page>
		<v-ons-toolbar>
			<div class="left">
				<v-ons-toolbar-button>
					<v-ons-back-button></v-ons-back-button>
				</v-ons-toolbar-button>
			</div>
			<div class="center">Auftrag Details</div>
		</v-ons-toolbar>

        <section style="margin: 20px">
            <p style="font-size: 20px; margin-top: 20px; color: #000;"><b>Auftraggeber:</b> {{name}}</p>
            <br />
            <div style="font-weight:bold;">Nachricht:</div>
            <div>{{message}}</div>
            <v-ons-fab modifier="mini" style="background-color:darkgreen;color:white;" position='bottom left' v-on:click="acceptService">
                <ons-icon icon="fa-check"></ons-icon>
            </v-ons-fab>
            <v-ons-fab modifier="mini" style="background-color:red;color:white;" position='bottom right' v-on:click="deleteService">
                <ons-icon icon="fa-times"></ons-icon>
            </v-ons-fab>
        </section>
	</v-ons-page>
</template>

<script>
import Apps from "../../apps"

export default Apps.register({
    name: "TaxiServiceContactApp",
	data() {
		return {
			name: "",
			message: "",
			spielerId: null
		}
	},
	methods: {
		acceptService() {
			if (this.name == "" || this.message == "" || this.spielerId == null) return
			this.triggerServer("acceptServiceTaxi", this.spielerId)
            this.pageStack.pop()
		},
		deleteService() {
		    if(this.name == "" || this.message == ""|| this.spielerId == null) return
            this.triggerServer("deleteServiceTaxi", this.spielerId)
            this.pageStack.pop()
		}
	},
	props: ['pageStack']
})
</script>